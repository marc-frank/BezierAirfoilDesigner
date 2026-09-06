# GN VarPro implementation

The port follows `spline_fitter/optimization/optimizers/gn_varpro.py`, with a
Bernstein basis in place of a clamped B-spline basis. A degree-p Bézier curve is
the degree-p clamped B-spline with p+1 control points and no interior knots.
The source MIT notice is retained in `THIRD-PARTY-NOTICES.md`.

## Objective and continuity

For ordered section parameters t, solve the linear constrained problem

    minimize_P ||N(t) P - D||² + smoothingWeight * trace(Pᵀ S P)
                                + regularization * ||P||²
    subject to C P = d.

The KKT system uses NᵀN + smoothingWeight*S + regularization*I. S is the integral
of products of second derivatives, normalized to trace(S) = control-point count
separately for each section. Bernstein product integrals are evaluated exactly,
including at degrees beyond the source's eight-point quadrature accuracy.

As in the Python method, the outer LM objective is the **data residual alone**,
`r(t) = N(t) P*(t) - D`. Smoothing and the numerical ridge belong to the inner
linear solve. The reduced Jacobian differentiates that same KKT system:

    dP*/dt_j = KKT_solve(-N'_j r_j - N_j (N'_j P*), 0)
    dr_i/dt_j = delta_ij N'_i P* + N_i dP*/dt_j.

The coupling term is retained. Each evaluation's LU factorization is reused
for all sensitivity right-hand sides. The analytic Jacobian is the default;
central differences are also available as a diagnostic fallback.

The first and last endpoints of the complete traversal equal their reference
data points, so an open trailing-edge gap is preserved. At every internal join:

| Mode | Matching conditions | Minimum control points per joined section |
| --- | --- | --- |
| G0 | Position | 2 |
| G1 | Position and first derivative | 3 |
| G2 | Position, first and second derivatives | 4 |
| G3 | Position, first, second and third derivatives | 5 |

These names and minimum counts follow the source. G1–G3 enforce parametric C1–C3
conditions, which imply the corresponding geometric continuity at regular
joins. Endpoint derivatives include each section's actual degree factors.
Fixed joins equal the final data point of the preceding section; free joins
omit that positional anchor but retain all matching conditions. There is no
extra vertical-tangent constraint or trailing-edge closure constraint in the
source method, and none is added here.

## Parameters and stopping

Initialization uses centripetal spacing. Positive softplus gaps and normalized
cumulative sums keep each section's parameters ordered in [0,1], with endpoints
exactly 0 and 1. Repeated data points are supported. As with the source, gaps can
coincide at floating-point precision. The chain rule applies the gap Jacobian to
the complete reduced residual Jacobian.

LM uses adaptive column scaling and gain-based damping. Unlike SciPy's
evaluation-budget multiplier, `MaxIterations` directly limits accepted LM
steps (50 by default), with at most 24 damping trials per step. Rejected steps
do not replace the saved solution. Gradient, step and residual tolerances default
to 1e-15. A positive target RMS, cancellation token, or callback can stop early.
Cancellation before the first solve throws; cancellation afterward returns the
best accepted constrained solution. Numerical/validation failures are reported
instead of silently returning an apparently successful fit.

`GnVarProFitter.Fit` accepts an ordered list of sections and a control-point count
per section, plus optional settings, cancellation, progress/stop callback, and
custom initial parameter arrays. It returns per-section control points and
parameters, accepted-step RMS/time history, iteration/evaluation counts, and a
stopping status. The generic engine supports multiple joins; the designer uses
its existing two-surface geometry. The UI reverses the upper surface before
fitting and restores its LE-to-TE direction on application. Both polygons, curve
samples, grids and export control points are updated together on the UI thread.

## Validation

Run `dotnet run --project BezierAirfoilDesigner.Tests` on Windows. This is a
standalone assertion runner; a nonzero exit code indicates failure. It covers:

- Python-generated control-point, residual and reduced-Jacobian fixtures for
  every G0–G3 mode, fixed/free joins, and smoothing on/off.
- Independent finite differences, mixed-degree endpoint derivatives, and
  multiple-section constraints through G3.
- Exact cubic recovery, real RG15 data, repeated/constant data, two-point
  sections, smoothing invariants, invalid inputs and stopping behavior.
- Editor orientation, synchronized model/grids/export data, and fit-button
  availability during an existing fit.

The fixture generator calls the original Python code directly. To regenerate:

    python BezierAirfoilDesigner.Tests/generate_python_fixture.py PATH_TO_SPLINE_FITTER

Python, NumPy and SciPy are required only to regenerate fixtures, not to run the
application or its .NET checks. The RG15 regression uses degree 6 on each surface,
G2 with a fixed leading edge, smoothing 1e-3 and a maximum of 50 LM iterations.
