using BezierAirfoilDesigner.Fitting;

namespace BezierAirfoilDesigner;

public partial class Form1
{
    private void InitializeGnVarPro()
    {
        var button = new System.Windows.Forms.Button
        {
            Name = "btnGnVarPro", Text = "GN VarPro...", AutoSize = true,
            Location = new Point(6, 245), Enabled = false, TabIndex = 78
        };
        toolTip.SetToolTip(button, "Fit both surfaces together with G0-G3 continuity at the leading edge.");
        // Existing fit buttons share their enabled state during loading/search/PSO.
        btnSearchTop.EnabledChanged += (_, _) => button.Enabled = btnSearchTop.Enabled;
        button.Click += (_, _) => ShowGnVarPro();
        tabPageCurveFitting.Controls.Add(button);
    }

    private void ShowGnVarPro()
    {
        if (referenceDatTop.Count < 2 || referenceDatBottom.Count < 2)
        {
            MessageBox.Show(this, "Load a reference .dat airfoil with both surfaces first.", "GN VarPro");
            return;
        }
        dataGridViewTop.EndEdit();
        dataGridViewBottom.EndEdit();
        // The editor stores both surfaces LE -> TE. The solver needs one
        // continuous traversal, TE -> upper -> LE -> lower -> TE.
        IReadOnlyList<PointD>[] sections =
        {
            referenceDatTop.AsEnumerable().Reverse().Select(p => new PointD(p.X, p.Y)).ToArray(),
            referenceDatBottom.Select(p => new PointD(p.X, p.Y)).ToArray()
        };
        var counts = new[] { GetControlPoints(dataGridViewTop).Count, GetControlPoints(dataGridViewBottom).Count };
        using var dialog = new GnVarProDialog(sections, counts);
        if (dialog.ShowDialog(this) != DialogResult.OK || dialog.Result == null) return;
        ApplyGnVarProResult(dialog.Result);
    }

    private void ApplyGnVarProResult(GnVarProResult result)
    {
        var top = result.ControlPoints[0].AsEnumerable().Reverse().Select(p => new PointD(p.X, p.Y)).ToList();
        var bottom = result.ControlPoints[1].Select(p => new PointD(p.X, p.Y)).ToList();
        // Apply the pair together; grid events must not recalculate a half-written fit.
        dataGridViewTop.CellValueChanged -= dataGridView1_CellValueChanged;
        dataGridViewBottom.CellValueChanged -= dataGridView2_CellValueChanged;
        try
        {
            controlPointsTop = top;
            controlPointsBottom = bottom;
            airfoilList[0].ControlPointsTop = top;
            airfoilList[0].ControlPointsBottom = bottom;
            pointsTop = airfoilList[0].CurvePointsTop;
            gridViewAddPoints(dataGridViewTop, top);
            gridViewAddPoints(dataGridViewBottom, bottom);
        }
        finally
        {
            dataGridViewTop.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridViewBottom.CellValueChanged += dataGridView2_CellValueChanged;
        }
        calculations();
        txtAirfoilParam.AppendText($"{Environment.NewLine}GN VarPro RMS:\t{result.RmsError:G6}{Environment.NewLine}{result.Message}");
    }
}
