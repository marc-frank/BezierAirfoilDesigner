using System.Globalization;
using System.Reflection;
using System.Text.Json;
using BezierAirfoilDesigner;
using BezierAirfoilDesigner.Fitting;
using MathNet.Numerics.LinearAlgebra;

namespace BezierAirfoilDesigner.Tests;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        var tests = new Action[] { PythonParity, Jacobian, Continuity, MultipleSections, SyntheticRecovery,
            RealAirfoil, Lifecycle, DegenerateData, Smoothing, EditorIntegration };
        try
        {
            foreach (var test in tests) { test(); Console.WriteLine("PASS " + test.Method.Name); }
            Console.WriteLine($"PASS: {tests.Length} test groups.");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }
    private static void Near(double actual, double expected, double tolerance, string message) =>
        Check(double.IsFinite(actual) && Math.Abs(actual - expected) <= tolerance,
            $"{message}: {actual:G17} vs {expected:G17}, tolerance {tolerance}");
    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); } catch (T) { return; }
        throw new Exception("Expected " + typeof(T).Name);
    }
    private static IReadOnlyList<PointD>[] Airfoil(int n = 21)
    {
        var upper = new List<PointD>(); var lower = new List<PointD>();
        for (int i = 0; i < n; i++)
        {
            double x = (1 - Math.Cos(Math.PI * i / (n - 1))) / 2;
            double y = 0.6 * (0.2969 * Math.Sqrt(x) - 0.126 * x - 0.3516 * x*x + 0.2843 * x*x*x - 0.1015 * Math.Pow(x, 4));
            double camber = 0.02 * x * (1 - x);
            upper.Add(new PointD(x, y + camber)); lower.Add(new PointD(x, -y + camber));
        }
        upper.Reverse();
        return new IReadOnlyList<PointD>[] { upper, lower };
    }
    private static PointD Derivative(IReadOnlyList<PointD> controls, double t, int order)
    {
        // Independent oracle: difference the polygon and use de Casteljau.
        var p = controls.Select(p => new PointD(p.X, p.Y)).ToList();
        for (int k = 0; k < order; k++)
        {
            int degree = p.Count - 1;
            p = Enumerable.Range(0, degree).Select(i => new PointD(
                degree * (p[i + 1].X - p[i].X), degree * (p[i + 1].Y - p[i].Y))).ToList();
        }
        while (p.Count > 1)
            p = p.Zip(p.Skip(1), (a, b) => new PointD((1-t)*a.X + t*b.X, (1-t)*a.Y + t*b.Y)).ToList();
        return p[0];
    }
    private static void PointNear(PointD actual, PointD expected, double tol, string message)
    {
        Near(actual.X, expected.X, tol, message + " X"); Near(actual.Y, expected.Y, tol, message + " Y");
    }
    private static void Validate(GnVarProResult result, IReadOnlyList<IReadOnlyList<PointD>> data, ContinuityMode mode, bool free)
    {
        PointNear(result.ControlPoints[0][0], data[0][0], 1e-9, "first TE");
        PointNear(result.ControlPoints[^1][^1], data[^1][^1], 1e-9, "last TE");
        for (int s = 0; s < data.Count - 1; s++)
        {
            for (int k = 0; k <= (int)mode; k++)
                PointNear(Derivative(result.ControlPoints[s], 1, k), Derivative(result.ControlPoints[s+1], 0, k), 2e-8, "junction derivative " + k);
            if (!free) PointNear(result.ControlPoints[s][^1], data[s][^1], 1e-9, "fixed junction");
        }
        foreach (var t in result.Parameters)
        {
            Near(t[0], 0, 0, "first parameter"); Near(t[^1], 1, 0, "last parameter");
            Check(t.All(double.IsFinite) && t.Zip(t.Skip(1)).All(p => p.First <= p.Second), "monotone parameters");
        }
        Check(result.History.Zip(result.History.Skip(1)).All(p => p.Second.RmsError <= p.First.RmsError), "accepted RMS must not increase");
    }
    private static void PythonParity()
    {
        using var json = JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures/python-varpro.json")));
        foreach (var test in json.RootElement.EnumerateArray())
        {
            var data = test.GetProperty("sections").EnumerateArray().Select(s => (IReadOnlyList<PointD>)s.EnumerateArray()
                .Select(p => new PointD(p[0].GetDouble(), p[1].GetDouble())).ToArray()).ToArray();
            var parameters = test.GetProperty("parameters").EnumerateArray().Select(p => p.EnumerateArray().Select(x => x.GetDouble()).ToArray()).ToArray();
            var problem = new VarProProblem(data, new[] { 5, 5 }, new GnVarProOptions
            {
                Continuity = Enum.Parse<ContinuityMode>(test.GetProperty("mode").GetString()!),
                FreeJunctions = test.GetProperty("free").GetBoolean(), SmoothingWeight = test.GetProperty("smoothing").GetDouble()
            });
            var eta = problem.InitialEta(parameters); var state = problem.Evaluate(eta); var jacobian = problem.Jacobian(state, eta);
            for (int i = 0; i < 10; i++)
                for (int axis = 0; axis < 2; axis++)
                    Near(state.Controls[i, axis], test.GetProperty("controls")[i][axis].GetDouble(), 2e-9, "Python control point");
            for (int i = 0; i < state.Residual.Count; i++)
            {
                Near(state.Residual[i], test.GetProperty("residual")[i].GetDouble(), 2e-10, "Python residual");
                for (int j = 0; j < eta.Count; j++)
                    Near(jacobian[i,j], test.GetProperty("jacobian")[i][j].GetDouble(), 2e-9, "Python reduced Jacobian");
            }
        }
    }
    private static void Jacobian()
    {
        foreach (var mode in Enum.GetValues<ContinuityMode>())
            foreach (bool free in new[] { false, true })
            {
                var problem = new VarProProblem(Airfoil(13), new[] { 5, 7 }, new GnVarProOptions { Continuity = mode, FreeJunctions = free, SmoothingWeight = 0.013 });
                var eta = problem.InitialEta(); eta[5] += 0.2;
                var state = problem.Evaluate(eta); var analytic = problem.Jacobian(state, eta);
                int evaluations = 0;
                var numeric = problem.FiniteDifferenceJacobian(state, eta, default, ref evaluations);
                Check((analytic - numeric).FrobeniusNorm() < 2e-7, "finite difference Jacobian " + mode);
            }
    }
    private static void Continuity()
    {
        foreach (var mode in Enum.GetValues<ContinuityMode>())
            foreach (bool free in new[] { false, true })
            {
                var data = Airfoil();
                var result = GnVarProFitter.Fit(data, new[] { 5, 7 }, new GnVarProOptions { Continuity = mode, FreeJunctions = free, MaxIterations = 15 });
                Validate(result, data, mode, free);
                Check(result.RmsError < result.History[0].RmsError * 0.8, "LM should improve " + mode);
                if (free) Check(Math.Abs(result.ControlPoints[0][^1].X) + Math.Abs(result.ControlPoints[0][^1].Y) > 1e-8, "free LE should move");
            }
    }
    private static void MultipleSections()
    {
        var data = Enumerable.Range(0, 3).Select(s => (IReadOnlyList<PointD>)Enumerable.Range(0, 11)
            .Select(i => { double x = (s + i / 10.0) / 3; return new PointD(x, Math.Sin(x * 3)); }).ToArray()).ToArray();
        foreach (bool free in new[] { false, true })
            Validate(GnVarProFitter.Fit(data, new[] { 5, 6, 5 }, new GnVarProOptions { Continuity = ContinuityMode.G3, FreeJunctions = free, MaxIterations = 5 }), data, ContinuityMode.G3, free);
    }
    private static void SyntheticRecovery()
    {
        PointD[] polygon = { new(1, 0), new(0.5, 0.2), new(0, 0.1), new(0, 0) };
        var data = Enumerable.Range(0, 31).Select(i => Derivative(polygon, Math.Pow(i / 30.0, 1.3), 0)).ToArray();
        var result = GnVarProFitter.Fit(new[] { data }, new[] { 4 }, new GnVarProOptions { SmoothingWeight = 0, MaxIterations = 100, TargetRms = 1e-9 });
        Check(result.RmsError < 1e-8, "recover cubic curve, RMS " + result.RmsError);
        for (int i = 0; i < polygon.Length; i++) PointNear(result.ControlPoints[0][i], polygon[i], 2e-6, "recovered polygon");
        var fd = GnVarProFitter.Fit(new[] { data }, new[] { 4 }, new GnVarProOptions { SmoothingWeight = 0, MaxIterations = 100, TargetRms = 1e-8, UseExactJacobian = false });
        Check(fd.RmsError < 2e-8, "finite difference fitting fallback");
    }
    private static void RealAirfoil()
    {
        var points = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Fixtures/RG15mod.dat")).Skip(1)
            .Select(l => l.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)).Where(p => p.Length == 2)
            .Select(p => new PointD(double.Parse(p[0], CultureInfo.InvariantCulture), double.Parse(p[1], CultureInfo.InvariantCulture))).ToArray();
        int le = Enumerable.Range(0, points.Length).MinBy(i => Math.Abs(points[i].X) + Math.Abs(points[i].Y));
        IReadOnlyList<PointD>[] data = { points.Take(le + 1).ToArray(), points.Skip(le).ToArray() };
        var result = GnVarProFitter.Fit(data, new[] { 7, 7 }, new GnVarProOptions { MaxIterations = 50 });
        Validate(result, data, ContinuityMode.G2, false);
        Console.WriteLine($"  RG15: RMS {result.History[0].RmsError:G6} -> {result.RmsError:G6} in {result.Iterations} iterations");
        Check(result.RmsError < result.History[0].RmsError / 4 && result.RmsError < 0.001, "RG15 fit accuracy");
    }
    private static void Lifecycle()
    {
        var data = Airfoil(13);
        var stopped = GnVarProFitter.Fit(data, new[] { 5, 5 }, callback: p => p.Iteration == 2);
        Check(stopped.Stopped && stopped.Iterations == 2 && !stopped.Converged, "callback stopping");
        Validate(stopped, data, ContinuityMode.G2, false);
        using var source = new CancellationTokenSource();
        var cancelled = GnVarProFitter.Fit(data, new[] { 5, 5 }, cancellationToken: source.Token,
            callback: p => { if (p.Iteration == 1) source.Cancel(); return false; });
        Check(cancelled.Stopped && cancelled.Iterations == 1, "token cancellation");
        Throws<OperationCanceledException>(() => GnVarProFitter.Fit(data, new[] { 5, 5 }, cancellationToken: source.Token));
        var target = GnVarProFitter.Fit(data, new[] { 5, 5 }, new GnVarProOptions { TargetRms = 1 });
        Check(target.Converged && target.Iterations == 0, "target RMS");
        var limited = GnVarProFitter.Fit(data, new[] { 5, 5 }, new GnVarProOptions { MaxIterations = 1 });
        Check(limited.Iterations == 1 && !limited.Converged, "iteration limit");
        Throws<ArgumentException>(() => GnVarProFitter.Fit(data, new[] { 4, 4 }, new GnVarProOptions { Continuity = ContinuityMode.G3 }));
        Throws<ArgumentException>(() => GnVarProFitter.Fit(data, new[] { 5, 5 }, new GnVarProOptions { SmoothingWeight = double.NaN }));
        Throws<ArgumentException>(() => GnVarProFitter.Fit(new[] { new[] { new PointD(double.NaN, 0), new PointD(1, 1) } }, new[] { 2 }));
        Throws<ArgumentException>(() => GnVarProFitter.Fit(data, new[] { 5, 5 }, initialParameters: new[] { new[] { 0.0, 1.0 } }));
    }
    private static void DegenerateData()
    {
        var eta = Vector<double>.Build.DenseOfArray(new[] { -1000.0, -500.0, -30.0, 0.0, 30.0, 1000.0 });
        var t = ParameterMapping.ToParameters(eta);
        Check(t.All(double.IsFinite) && t.Zip(t.Skip(1)).All(p => p.First <= p.Second), "extreme monotone gaps");
        Check(ParameterMapping.Jacobian(eta, t).Enumerate().All(double.IsFinite), "extreme gap derivatives");
        var data = Airfoil(9).Select(s => (IReadOnlyList<PointD>)s.SelectMany(p => new[] { p, p }).ToArray()).ToArray();
        Validate(GnVarProFitter.Fit(data, new[] { 5, 5 }, new GnVarProOptions { MaxIterations = 3 }), data, ContinuityMode.G2, false);
        var constant = Enumerable.Repeat(new PointD(0.3, 0.2), 8).ToArray();
        Check(GnVarProFitter.Fit(new[] { constant }, new[] { 4 }).RmsError < 1e-9, "all coincident data");
        var line = new[] { new PointD(0, 0), new PointD(1, 0.2) };
        Check(GnVarProFitter.Fit(new[] { line }, new[] { 2 }).RmsError < 1e-12, "two-point section");
    }
    private static void Smoothing()
    {
        foreach (int degree in new[] { 2, 3, 9, 12 })
        {
            var matrix = Bernstein.BendingEnergy(degree);
            Near(matrix.Trace(), degree + 1, 1e-10, "normalized bending trace");
            var line = Vector<double>.Build.Dense(degree + 1, i => i / (double)degree);
            Check((matrix * line).L2Norm() < 1e-10, "line has zero bending energy");
            var p = Vector<double>.Build.Dense(degree + 1, i => Math.Sin(i));
            Check(p.DotProduct(matrix * p) >= 0, "nonnegative bending energy");
        }
    }
    private static void EditorIntegration()
    {
        using var form = new Form1(); // Do not show it or trigger its network update check.
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        object Field(string name) => typeof(Form1).GetField(name, flags)!.GetValue(form)!;
        void Invoke(string name, params object[] args) => typeof(Form1).GetMethod(name, flags)!.Invoke(form, args);
        var airfoils = (List<Airfoil>)Field("airfoilList"); airfoils.Add((Airfoil)Field("defaultAirfoil"));
        Invoke("AddDefaultPointsTop"); Invoke("AddDefaultPointsBottom");
        var data = Airfoil(13);
        var result = GnVarProFitter.Fit(data, new[] { 5, 6 }, new GnVarProOptions { MaxIterations = 3 });
        Invoke("ApplyGnVarProResult", result);
        PointNear(airfoils[0].ControlPointsTop[0], result.ControlPoints[0][^1], 0, "editor LE orientation");
        PointNear(airfoils[0].ControlPointsTop[^1], data[0][0], 1e-9, "editor upper TE");
        Check(((List<PointD>)Field("controlPointsTop")).SequenceEqual(airfoils[0].ControlPointsTop), "export controls synchronized");
        Check(((List<PointD>)Field("pointsTop")).Count == airfoils[0].CurvePointsTop.Count, "curve samples synchronized");
        Check(((DataGridView)Field("dataGridViewTop")).Rows.Count == 6, "upper grid updated");
        Check(((DataGridView)Field("dataGridViewBottom")).Rows.Count == 7, "lower grid updated");
        var search = (System.Windows.Forms.Button)Field("btnSearchTop");
        var gn = form.Controls.Find("btnGnVarPro", true).Single();
        search.Enabled = false; Check(!gn.Enabled, "GN disabled during other fit");
        search.Enabled = true; Check(gn.Enabled, "GN enabled with reference");
        using var dialog = new GnVarProDialog(data, new[] { 5, 6 });
        Check(dialog.Result == null && dialog.Controls.Count > 0, "dialog initialized");
    }
}
