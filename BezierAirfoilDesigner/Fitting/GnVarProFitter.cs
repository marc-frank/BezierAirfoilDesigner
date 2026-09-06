// Port of spline_fitter/optimization/optimizers/gn_varpro.py to Bernstein bases.
// See THIRD-PARTY-NOTICES.md for the source's MIT license.
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace BezierAirfoilDesigner.Fitting;

public static class GnVarProFitter
{
    /// <summary>
    /// Fits ordered sections, each running from the end of the previous one to
    /// the start of the next. The two outer endpoints are fixed to the data.
    /// Control points are eliminated by constrained least squares; LM optimizes
    /// positive parameter gaps using the exact reduced (coupled) Jacobian.
    /// </summary>
    public static GnVarProResult Fit(
        IReadOnlyList<IReadOnlyList<PointD>> sections,
        IReadOnlyList<int> controlPointCounts,
        GnVarProOptions? options = null,
        CancellationToken cancellationToken = default,
        Func<FitProgress, bool>? callback = null,
        IReadOnlyList<double[]>? initialParameters = null)
    {
        options ??= new GnVarProOptions();
        var problem = new VarProProblem(sections, controlPointCounts, options);
        var eta = problem.InitialEta(initialParameters);
        cancellationToken.ThrowIfCancellationRequested();
        var clock = Stopwatch.StartNew();
        var current = problem.Evaluate(eta);
        int evaluations = 1, iterations = 0;
        var history = new List<FitProgress>();
        bool stopped = false, converged = false;
        string message = "Iteration limit reached.";
        double damping = 1e-3;
        var scale = Vector<double>.Build.Dense(eta.Count, 1e-12);

        bool Report()
        {
            var progress = new FitProgress(iterations, evaluations,
                Math.Sqrt(current.Cost / problem.PointCount), clock.Elapsed);
            history.Add(progress);
            return callback?.Invoke(progress) == true || cancellationToken.IsCancellationRequested;
        }

        stopped = Report();
        try
        {
            while (!stopped && iterations < options.MaxIterations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (Math.Sqrt(current.Cost / problem.PointCount) <= options.TargetRms)
                {
                    converged = true;
                    message = "Target RMS reached.";
                    break;
                }
                var jacobian = options.UseExactJacobian
                    ? problem.Jacobian(current, eta, cancellationToken)
                    : problem.FiniteDifferenceJacobian(current, eta, cancellationToken, ref evaluations);
                var normal = jacobian.TransposeThisAndMultiply(jacobian);
                var gradient = jacobian.TransposeThisAndMultiply(current.Residual);
                double scaledGradient = 0;
                for (int i = 0; i < eta.Count; i++)
                {
                    scale[i] = Math.Max(scale[i], normal[i, i]);
                    scaledGradient = Math.Max(scaledGradient, Math.Abs(gradient[i]) / Math.Sqrt(scale[i]));
                }
                if (scaledGradient <= options.Tolerance * Math.Max(1, Math.Sqrt(current.Cost)))
                {
                    converged = true;
                    message = "Gradient tolerance reached.";
                    break;
                }

                bool accepted = false;
                double growth = 2;
                for (int trial = 0; trial < 24; trial++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var damped = normal.Clone();
                    for (int i = 0; i < eta.Count; i++)
                        damped[i, i] += damping * scale[i];
                    var step = damped.Cholesky().Solve(-gradient);
                    if (step.L2Norm() <= options.Tolerance * (options.Tolerance + eta.L2Norm()))
                    {
                        converged = true;
                        message = "Step tolerance reached.";
                        break;
                    }
                    var candidate = problem.Evaluate(eta + step);
                    evaluations++;
                    double reduction = current.Cost - candidate.Cost;
                    double predicted = -2 * gradient.DotProduct(step) - step.DotProduct(normal * step);
                    if (reduction > 0 && predicted > 0)
                    {
                        double oldCost = current.Cost;
                        eta += step;
                        current = candidate;
                        iterations++;
                        accepted = true;
                        double gain = reduction / predicted;
                        damping = Math.Max(1e-15, damping * Math.Max(1.0 / 3, 1 - Math.Pow(2 * gain - 1, 3)));
                        stopped = Report();
                        if (reduction <= options.Tolerance * oldCost)
                        {
                            converged = true;
                            message = "Residual tolerance reached.";
                        }
                        break;
                    }
                    damping = Math.Min(1e30, damping * growth);
                    growth = Math.Min(1e12, growth * 2);
                }
                if (converged) break;
                if (!accepted)
                {
                    message = "No improving step found; kept the best accepted fit.";
                    break;
                }
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            stopped = true;
        }
        stopped |= cancellationToken.IsCancellationRequested;
        if (stopped)
        {
            converged = false;
            message = "Stopped; kept the best accepted fit.";
        }
        return new GnVarProResult(problem.SplitControlPoints(current.Controls),
            current.Parameters, history, iterations, evaluations,
            Math.Sqrt(current.Cost / problem.PointCount), converged, stopped, message);
    }
}

internal sealed class VarProEvaluation
{
    internal Matrix<double> Basis { get; init; } = null!;
    internal Matrix<double> Derivatives { get; init; } = null!;
    internal Matrix<double> Controls { get; init; } = null!;
    internal Vector<double> Residual { get; init; } = null!;
    internal LU<double> Factorization { get; init; } = null!;
    internal double[][] Parameters { get; init; } = null!;
    internal double Cost => Residual.DotProduct(Residual);
}

internal sealed class VarProProblem
{
    private readonly GnVarProOptions options;
    private readonly PointD[][] sections;
    private readonly int[] counts, pointOffsets, controlOffsets;
    private readonly Matrix<double> data, constraints, constraintValues, penalty;
    internal int PointCount { get; }
    private int ControlCount { get; }

    internal VarProProblem(IReadOnlyList<IReadOnlyList<PointD>> sections,
        IReadOnlyList<int> counts, GnVarProOptions options)
    {
        if (sections == null || counts == null || sections.Count == 0 || sections.Count != counts.Count)
            throw new ArgumentException("Provide a control point count for each nonempty section.");
        if (!Enum.IsDefined(typeof(ContinuityMode), options.Continuity) || options.MaxIterations < 1 ||
            !double.IsFinite(options.Regularization) || options.Regularization <= 0 ||
            !double.IsFinite(options.SmoothingWeight) || options.SmoothingWeight < 0 ||
            !double.IsFinite(options.Tolerance) || options.Tolerance < 1e-15 || options.Tolerance >= 1 ||
            !double.IsFinite(options.TargetRms) || options.TargetRms < 0)
            throw new ArgumentException("Invalid fitting settings.");
        this.options = options;
        this.counts = counts.ToArray();
        this.sections = new PointD[sections.Count][];
        pointOffsets = new int[sections.Count];
        controlOffsets = new int[sections.Count];
        int order = (int)options.Continuity;
        for (int s = 0; s < sections.Count; s++)
        {
            if (sections[s] == null || sections[s].Count < 2 || counts[s] < 2)
                throw new ArgumentException("Each section needs at least two data points and two control points.");
            if (sections.Count > 1 && counts[s] < order + 2)
                throw new ArgumentException($"{options.Continuity} needs at least {order + 2} control points (degree {order + 1}) on each surface.");
            if (sections[s].Any(p => p == null || !double.IsFinite(p.X) || !double.IsFinite(p.Y)))
                throw new ArgumentException("Data coordinates must be finite numbers.");
            this.sections[s] = sections[s].Select(p => new PointD(p.X, p.Y)).ToArray();
            pointOffsets[s] = PointCount;
            controlOffsets[s] = ControlCount;
            PointCount += sections[s].Count;
            ControlCount += counts[s];
        }
        data = Matrix<double>.Build.Dense(PointCount, 2);
        for (int s = 0; s < sections.Count; s++)
            for (int i = 0; i < sections[s].Count; i++)
            {
                data[pointOffsets[s] + i, 0] = sections[s][i].X;
                data[pointOffsets[s] + i, 1] = sections[s][i].Y;
            }
        (constraints, constraintValues) = BuildConstraints();
        if (constraints.RowCount > ControlCount || constraints.Svd().Rank < constraints.RowCount)
            throw new ArgumentException("The selected degrees cannot satisfy all junction constraints independently.");
        penalty = Matrix<double>.Build.DenseIdentity(ControlCount) * options.Regularization;
        for (int s = 0; s < sections.Count; s++)
        {
            var bending = Bernstein.BendingEnergy(counts[s] - 1) * options.SmoothingWeight;
            for (int i = 0; i < counts[s]; i++)
                for (int j = 0; j < counts[s]; j++)
                    penalty[controlOffsets[s] + i, controlOffsets[s] + j] += bending[i, j];
        }
    }

    private (Matrix<double>, Matrix<double>) BuildConstraints()
    {
        var rows = new List<double[]>();
        var values = new List<PointD>();
        void Add(double[] row, PointD value)
        {
            // Scaling derivative constraints avoids ill-conditioning at high degrees.
            double norm = Math.Sqrt(row.Sum(x => x * x));
            rows.Add(row.Select(x => x / norm).ToArray());
            values.Add(new PointD(value.X / norm, value.Y / norm));
        }
        void Fix(int index, PointD value)
        {
            var row = new double[ControlCount];
            row[index] = 1;
            Add(row, value);
        }
        Fix(0, sections[0][0]);
        Fix(ControlCount - 1, sections[^1][^1]);
        for (int s = 0; s < sections.Length - 1; s++)
        {
            for (int order = 0; order <= (int)options.Continuity; order++)
            {
                var row = new double[ControlCount];
                var end = Bernstein.Derivative(counts[s] - 1, 1, order);
                var start = Bernstein.Derivative(counts[s + 1] - 1, 0, order);
                for (int i = 0; i < counts[s]; i++) row[controlOffsets[s] + i] = end[i];
                for (int i = 0; i < counts[s + 1]; i++) row[controlOffsets[s + 1] + i] = -start[i];
                Add(row, new PointD(0, 0));
            }
            if (!options.FreeJunctions) Fix(controlOffsets[s] + counts[s] - 1, sections[s][^1]);
        }
        return (Matrix<double>.Build.DenseOfRowArrays(rows),
            Matrix<double>.Build.Dense(values.Count, 2, (i, j) => j == 0 ? values[i].X : values[i].Y));
    }

    internal Vector<double> InitialEta(IReadOnlyList<double[]>? parameters = null)
    {
        if (parameters != null && parameters.Count != sections.Length)
            throw new ArgumentException("Initial parameters must include every section.");
        var eta = new List<double>();
        for (int s = 0; s < sections.Length; s++)
        {
            var t = parameters == null ? Centripetal(sections[s]) : parameters[s];
            if (t == null || t.Length != sections[s].Length || t[0] != 0 || t[^1] != 1 ||
                t.Any(x => !double.IsFinite(x) || x < 0 || x > 1) || t.Zip(t.Skip(1)).Any(p => p.First > p.Second))
                throw new ArgumentException("Initial parameters must be ordered from 0 to 1, one per data point.");
            for (int i = 1; i < t.Length; i++)
                eta.Add(ParameterMapping.InverseSoftplus(Math.Max(1e-15, t[i] - t[i - 1])));
        }
        return Vector<double>.Build.DenseOfEnumerable(eta);
    }

    private static double[] Centripetal(PointD[] points)
    {
        var t = new double[points.Length];
        for (int i = 1; i < t.Length; i++)
        {
            double dx = points[i].X - points[i - 1].X, dy = points[i].Y - points[i - 1].Y;
            t[i] = t[i - 1] + Math.Sqrt(Math.Sqrt(dx * dx + dy * dy));
        }
        double total = t[^1];
        if (!double.IsFinite(total)) throw new ArgumentException("Data coordinates are too large to fit.");
        for (int i = 1; i < t.Length; i++) t[i] = total > 0 ? t[i] / total : (double)i / (t.Length - 1);
        return t;
    }

    internal VarProEvaluation Evaluate(Vector<double> eta)
    {
        var basis = Matrix<double>.Build.Dense(PointCount, ControlCount);
        var derivatives = Matrix<double>.Build.Dense(PointCount, ControlCount);
        var parameters = new double[sections.Length][];
        int offset = 0;
        for (int s = 0; s < sections.Length; s++)
        {
            int size = sections[s].Length;
            var t = ParameterMapping.ToParameters(eta.SubVector(offset, size - 1));
            parameters[s] = t;
            offset += size - 1;
            for (int i = 0; i < size; i++)
            {
                var b = Bernstein.Basis(counts[s] - 1, t[i]);
                var db = Bernstein.Derivative(counts[s] - 1, t[i], 1);
                for (int j = 0; j < counts[s]; j++)
                {
                    basis[pointOffsets[s] + i, controlOffsets[s] + j] = b[j];
                    derivatives[pointOffsets[s] + i, controlOffsets[s] + j] = db[j];
                }
            }
        }
        int kktSize = ControlCount + constraints.RowCount;
        var kkt = Matrix<double>.Build.Dense(kktSize, kktSize);
        kkt.SetSubMatrix(0, 0, basis.TransposeThisAndMultiply(basis) + penalty);
        kkt.SetSubMatrix(ControlCount, 0, constraints);
        kkt.SetSubMatrix(0, ControlCount, constraints.Transpose());
        var rhs = Matrix<double>.Build.Dense(kktSize, 2);
        rhs.SetSubMatrix(0, 0, basis.TransposeThisAndMultiply(data));
        rhs.SetSubMatrix(ControlCount, 0, constraintValues);
        var factor = kkt.LU();
        var controls = factor.Solve(rhs).SubMatrix(0, ControlCount, 0, 2);
        var residual2d = basis * controls - data;
        var residual = Vector<double>.Build.Dense(2 * PointCount, i => residual2d[i / 2, i % 2]);
        if (controls.Enumerate().Any(x => !double.IsFinite(x)) || !double.IsFinite(residual.DotProduct(residual)))
            throw new ArithmeticException("The constrained fit is numerically singular. Reduce the degree or increase regularization.");
        return new VarProEvaluation { Basis = basis, Derivatives = derivatives, Controls = controls,
            Residual = residual, Parameters = parameters, Factorization = factor };
    }

    internal Matrix<double> Jacobian(VarProEvaluation state, Vector<double> eta, CancellationToken token = default)
    {
        // Differentiate the same regularized KKT system used to eliminate P.
        // RHS_j = -N'_j r_j - N_j (N'_j P), with zero constraint derivatives.
        var direct = state.Derivatives * state.Controls;
        var rhs = Matrix<double>.Build.Dense(ControlCount + constraints.RowCount, 2 * PointCount);
        for (int j = 0; j < PointCount; j++)
        {
            token.ThrowIfCancellationRequested();
            for (int i = 0; i < ControlCount; i++)
                for (int axis = 0; axis < 2; axis++)
                    rhs[i, 2 * j + axis] = -state.Derivatives[j, i] * state.Residual[2 * j + axis]
                        - state.Basis[j, i] * direct[j, axis];
        }
        var sensitivity = state.Factorization.Solve(rhs).SubMatrix(0, ControlCount, 0, 2 * PointCount);
        var coupling = state.Basis * sensitivity;
        var jt = Matrix<double>.Build.Dense(2 * PointCount, PointCount);
        for (int i = 0; i < PointCount; i++)
            for (int j = 0; j < PointCount; j++)
                for (int axis = 0; axis < 2; axis++)
                    jt[2 * i + axis, j] = coupling[i, 2 * j + axis] + (i == j ? direct[i, axis] : 0);
        var result = Matrix<double>.Build.Dense(2 * PointCount, eta.Count);
        int offset = 0;
        for (int s = 0; s < sections.Length; s++)
        {
            token.ThrowIfCancellationRequested();
            int size = sections[s].Length;
            var g = ParameterMapping.Jacobian(eta.SubVector(offset, size - 1), state.Parameters[s]);
            result.SetSubMatrix(0, offset, jt.SubMatrix(0, 2 * PointCount, pointOffsets[s], size) * g);
            offset += size - 1;
        }
        return result;
    }

    internal Matrix<double> FiniteDifferenceJacobian(VarProEvaluation state, Vector<double> eta,
        CancellationToken token, ref int evaluations)
    {
        var result = Matrix<double>.Build.Dense(2 * PointCount, eta.Count);
        for (int j = 0; j < eta.Count; j++)
        {
            token.ThrowIfCancellationRequested();
            double step = 1e-6 * Math.Max(1, Math.Abs(eta[j]));
            var plus = eta.Clone();
            var minus = eta.Clone();
            plus[j] += step;
            minus[j] -= step;
            result.SetColumn(j, (Evaluate(plus).Residual - Evaluate(minus).Residual) / (2 * step));
            evaluations += 2;
        }
        return result;
    }

    internal IReadOnlyList<List<PointD>> SplitControlPoints(Matrix<double> controls) =>
        counts.Select((count, s) => Enumerable.Range(0, count)
            .Select(i => new PointD(controls[controlOffsets[s] + i, 0], controls[controlOffsets[s] + i, 1])).ToList()).ToArray();
}

internal static class ParameterMapping
{
    internal static double Softplus(double x) => x > 20 ? x : x < -20 ? Math.Exp(Math.Max(-500, x)) : Math.Log(1 + Math.Exp(x));
    internal static double InverseSoftplus(double y) => y < 1e-5 ? Math.Log(y) + y / 2 : y > 20 ? y : Math.Log(Math.Exp(y) - 1);
    private static double Sigmoid(double x)
    {
        if (x < -500) return 0; // derivative of the underflow guard
        if (x > 20) return 1;
        if (x < -20) return Math.Exp(x);
        double exp = Math.Exp(-Math.Abs(x));
        return x >= 0 ? 1 / (1 + exp) : exp / (1 + exp);
    }
    internal static double[] ToParameters(Vector<double> eta)
    {
        if (eta.Count == 0 || eta.Any(x => !double.IsFinite(x)))
            throw new ArgumentException("Parameter gaps must be finite and nonempty.");
        var t = new double[eta.Count + 1];
        for (int i = 0; i < eta.Count; i++) t[i + 1] = t[i] + Softplus(eta[i]);
        double sum = t[^1];
        if (!double.IsFinite(sum)) throw new ArithmeticException("Parameter gaps overflowed.");
        for (int i = 1; i < eta.Count; i++) t[i] /= sum;
        t[^1] = 1;
        return t;
    }
    internal static Matrix<double> Jacobian(Vector<double> eta, double[] t)
    {
        double sum = eta.Sum(Softplus);
        return Matrix<double>.Build.Dense(t.Length, eta.Count,
            (i, j) => Sigmoid(eta[j]) / sum * ((j < i ? 1 : 0) - t[i]));
    }
}

internal static class Bernstein
{
    internal static double[] Basis(int degree, double t)
    {
        var b = new double[degree + 1];
        b[0] = 1;
        for (int k = 1; k <= degree; k++)
        {
            for (int j = k; j > 0; j--) b[j] = (1 - t) * b[j] + t * b[j - 1];
            b[0] *= 1 - t;
        }
        return b;
    }
    internal static double[] Derivative(int degree, double t, int order)
    {
        if (order == 0) return Basis(degree, t);
        var result = new double[degree + 1];
        if (order > degree) return result;
        var b = Basis(degree - order, t);
        double factor = 1;
        for (int k = 0; k < order; k++) factor *= degree - k;
        for (int i = 0; i < b.Length; i++)
            for (int k = 0; k <= order; k++)
                result[i + k] += factor * Choose(order, k) * ((order - k) % 2 == 0 ? 1 : -1) * b[i];
        return result;
    }
    private static double Choose(int n, int k)
    {
        double result = 1;
        for (int i = 1; i <= Math.Min(k, n - k); i++) result *= (double)(n - i + 1) / i;
        return result;
    }
    internal static Matrix<double> BendingEnergy(int degree)
    {
        var result = Matrix<double>.Build.Dense(degree + 1, degree + 1);
        if (degree < 2) return result;
        int q = degree - 2;
        var differences = Matrix<double>.Build.Dense(q + 1, degree + 1);
        for (int i = 0; i <= q; i++)
        {
            differences[i, i] = differences[i, i + 2] = degree * (degree - 1);
            differences[i, i + 1] = -2 * degree * (degree - 1);
        }
        // Exact integral of B_i^q B_j^q on [0,1], including degrees above 9.
        var gram = Matrix<double>.Build.Dense(q + 1, q + 1,
            (i, j) => Choose(q, i) * Choose(q, j) / ((2 * q + 1) * Choose(2 * q, i + j)));
        result = differences.TransposeThisAndMultiply(gram * differences);
        return result * ((degree + 1) / result.Trace());
    }
}
