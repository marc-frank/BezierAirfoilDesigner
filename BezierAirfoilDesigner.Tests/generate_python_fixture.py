"""Regenerate parity fixtures: python generate_python_fixture.py PATH_TO_SPLINE_FITTER.

Calls the original Python implementation. NumPy and SciPy must be installed.
The checked-in JSON makes the .NET tests standalone.
"""
import json
from pathlib import Path
import sys
import re
import numpy as np

sys.path.insert(0, str(Path(sys.argv[1]) / "src"))
from spline_fitter.optimization.optimizers.gn_varpro import (
    VarProProblem, _sections_t_to_eta, _sections_eta_to_t,
    _build_reparam_jacobian_block,
)
from spline_fitter.optimization.models import ContinuityMode
from spline_fitter.geometry import centripetal_parametrize

x = (1 - np.cos(np.linspace(0, np.pi, 13))) / 2
y = 0.6 * (0.2969 * np.sqrt(x) - 0.1260 * x - 0.3516 * x**2 + 0.2843 * x**3 - 0.1015 * x**4)
sections = [np.column_stack([x[::-1], (y + 0.02 * x * (1-x))[::-1]]),
            np.column_stack([x, -y + 0.02 * x * (1-x)])]
knots = np.array([0.] * 5 + [1.] * 5)
cases = []
for mode in ContinuityMode:
    for free in (False, True):
        smoothing = 1e-3 if free else 0.
        problem = VarProProblem(sections, knots, 4, 5, mode, free, reg=1e-12, smoothing_weight=smoothing)
        parameters = [np.array(centripetal_parametrize(sec.tolist())) for sec in sections]
        eta, _ = _sections_t_to_eta(parameters)
        t, mapped = _sections_eta_to_t(eta, [13, 13])
        residual = problem.compute_residual(t)
        jt = problem.compute_jacobian(t)
        g = np.zeros((26, 24))
        for s in range(2):
            g[s*13:(s+1)*13, s*12:(s+1)*12] = _build_reparam_jacobian_block(eta[s*12:(s+1)*12], mapped[s])
        cases.append(dict(mode=mode.name, free=free, smoothing=smoothing,
                          sections=[s.tolist() for s in sections], parameters=[p.tolist() for p in parameters],
                          controls=problem.compute_optimal_control_points(t).tolist(),
                          residual=residual.tolist(), jacobian=(jt @ g).tolist()))
destination = Path(__file__).parent / "Fixtures" / "python-varpro.json"
destination.parent.mkdir(exist_ok=True)
# Keep numeric rows on one line so generated matrices remain reviewable.
rendered = re.sub(r'\[\s+([-+0-9.eE,\s]+)\s+\]',
                  lambda m: '[' + ' '.join(m[1].split()) + ']', json.dumps(cases, indent=2))
destination.write_text(rendered + "\n", encoding="utf-8")
print(f"Wrote {len(cases)} parity cases to {destination}")
