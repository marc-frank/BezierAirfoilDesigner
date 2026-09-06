namespace BezierAirfoilDesigner.Fitting;

// Names follow spline_fitter: G1-G3 enforce equality of derivative vectors
// (parametric C1-C3), which also implies geometric continuity at regular joins.
public enum ContinuityMode { G0, G1, G2, G3 }

public sealed class GnVarProOptions
{
    public ContinuityMode Continuity { get; init; } = ContinuityMode.G2;
    public bool FreeJunctions { get; init; }
    public int MaxIterations { get; init; } = 50;
    public double Regularization { get; init; } = 1e-12;
    public double SmoothingWeight { get; init; } = 1e-3;
    public bool UseExactJacobian { get; init; } = true;
    public double Tolerance { get; init; } = 1e-15;
    public double TargetRms { get; init; }
}

public sealed record FitProgress(int Iteration, int Evaluations, double RmsError, TimeSpan Elapsed);

public sealed record GnVarProResult(
    IReadOnlyList<List<PointD>> ControlPoints,
    IReadOnlyList<double[]> Parameters,
    IReadOnlyList<FitProgress> History,
    int Iterations,
    int Evaluations,
    double RmsError,
    bool Converged,
    bool Stopped,
    string Message);
