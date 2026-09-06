using System.Globalization;

namespace BezierAirfoilDesigner.Fitting;

internal sealed class GnVarProDialog : Form
{
    private readonly IReadOnlyList<IReadOnlyList<PointD>> sections;
    private readonly int[] counts;
    private readonly ComboBox continuity = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
    private readonly CheckBox freeJunctions = new() { Text = "Allow the leading edge to move", AutoSize = true };
    private readonly CheckBox exactJacobian = new() { Text = "Use analytic Jacobian", Checked = true, AutoSize = true };
    private readonly NumericUpDown iterations = new() { Minimum = 1, Maximum = 10000, Value = 50, Dock = DockStyle.Fill };
    private readonly TextBox smoothing = new() { Text = "1e-3", Dock = DockStyle.Fill };
    private readonly TextBox regularization = new() { Text = "1e-12", Dock = DockStyle.Fill };
    private readonly TextBox targetRms = new() { Text = "0", Dock = DockStyle.Fill };
    private readonly Label status = new() { AutoSize = true, MaximumSize = new Size(440, 0), Text = "Ready to fit both surfaces." };
    private readonly System.Windows.Forms.Button fit = new() { Text = "Fit both", AutoSize = true };
    private readonly System.Windows.Forms.Button stop = new() { Text = "Stop", Enabled = false, AutoSize = true };
    private readonly System.Windows.Forms.Button apply = new() { Text = "Apply fit", Enabled = false, AutoSize = true };
    private readonly System.Windows.Forms.Button close = new() { Text = "Close", AutoSize = true };
    private readonly TableLayoutPanel settings = new() { AutoSize = true, ColumnCount = 2, Dock = DockStyle.Top };
    private readonly ProgressBar progress = new() { Dock = DockStyle.Top, Height = 18 };
    private CancellationTokenSource? cancellation;
    private bool running;
    private bool closeWhenStopped;
    private int runId;
    internal GnVarProResult? Result { get; private set; }

    internal GnVarProDialog(IReadOnlyList<IReadOnlyList<PointD>> sections, int[] counts)
    {
        this.sections = sections;
        this.counts = counts;
        Text = "GN VarPro fitting";
        Font = new Font("Segoe UI", 9);
        AutoScaleMode = AutoScaleMode.Dpi;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = MinimizeBox = false;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(12);
        continuity.Items.AddRange(new object[] { "G0 - position", "G1 - tangent", "G2 - curvature", "G3 - curvature rate" });
        continuity.SelectedIndex = Math.Clamp(Math.Min(counts[0], counts[1]) - 2, 0, 2);
        settings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
        settings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260));
        void Row(string label, System.Windows.Forms.Control control)
        {
            int row = settings.RowCount++;
            settings.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row);
            settings.Controls.Add(control, 1, row);
        }
        Row("Leading-edge continuity", continuity);
        Row("Junction position", freeJunctions);
        Row("Maximum LM iterations", iterations);
        Row("Smoothing weight", smoothing);
        Row("Numerical regularization", regularization);
        Row("Target RMS (0 = disabled)", targetRms);
        Row("Derivatives", exactJacobian);
        var layout = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Dock = DockStyle.Fill };
        layout.Controls.Add(new Label
        {
            Text = $"Bézier degree: upper {counts[0] - 1}, lower {counts[1] - 1}. Both trailing-edge points stay at the reference data.\n" +
                "G1-G3 match derivative vectors, as in spline_fitter. G3 requires degree 4 or higher on both surfaces.",
            AutoSize = true, MaximumSize = new Size(440, 0), Margin = new Padding(3, 3, 3, 12)
        });
        layout.Controls.Add(settings);
        layout.Controls.Add(progress);
        layout.Controls.Add(status);
        var buttons = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, Margin = new Padding(0, 12, 0, 0) };
        buttons.Controls.AddRange(new System.Windows.Forms.Control[] { fit, stop, apply, close });
        layout.Controls.Add(buttons);
        Controls.Add(layout);
        fit.Click += async (_, _) => await RunFitAsync();
        stop.Click += (_, _) => cancellation?.Cancel();
        apply.Click += (_, _) => { DialogResult = DialogResult.OK; Close(); };
        close.Click += (_, _) => Close();
        FormClosing += (_, e) =>
        {
            if (!running) return;
            closeWhenStopped = true;
            cancellation?.Cancel();
            e.Cancel = true;
        };
        AcceptButton = fit;
        CancelButton = close;
    }

    private static double Number(TextBox input, string name, bool positive = false)
    {
        if (!double.TryParse(input.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out double value) ||
            !double.IsFinite(value) || (positive ? value <= 0 : value < 0))
            throw new ArgumentException($"{name} must be a {(positive ? "positive" : "nonnegative")} finite number.");
        return value;
    }

    private async Task RunFitAsync()
    {
        GnVarProOptions options;
        try
        {
            options = new GnVarProOptions
            {
                Continuity = (ContinuityMode)continuity.SelectedIndex,
                FreeJunctions = freeJunctions.Checked,
                MaxIterations = (int)iterations.Value,
                SmoothingWeight = Number(smoothing, "Smoothing weight"),
                Regularization = Number(regularization, "Regularization", true),
                TargetRms = Number(targetRms, "Target RMS"),
                UseExactJacobian = exactJacobian.Checked
            };
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(this, ex.Message, "GN VarPro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        Result = null;
        running = true;
        int thisRun = ++runId;
        settings.Enabled = fit.Enabled = apply.Enabled = false;
        stop.Enabled = true;
        progress.Value = 0;
        progress.Maximum = options.MaxIterations;
        status.Text = "Fitting both surfaces...";
        using var source = new CancellationTokenSource();
        cancellation = source;
        var updates = new Progress<FitProgress>(p =>
        {
            if (!running || IsDisposed || thisRun != runId) return;
            progress.Value = Math.Min(progress.Maximum, p.Iteration);
            status.Text = $"Iteration {p.Iteration}   RMS {p.RmsError:G6}   Time {p.Elapsed:hh\\:mm\\:ss}";
        });
        try
        {
            Result = await Task.Run(() => GnVarProFitter.Fit(sections, counts, options, source.Token,
                p => { ((IProgress<FitProgress>)updates).Report(p); return false; }));
            status.Text = $"{Result.Message}\nRMS {Result.RmsError:G6}; {Result.Iterations} iterations, {Result.Evaluations} evaluations.";
            apply.Enabled = true;
        }
        catch (OperationCanceledException)
        {
            status.Text = "Stopped before an initial fit was available.";
        }
        catch (Exception ex)
        {
            status.Text = "Fit failed. " + ex.Message;
        }
        finally
        {
            running = false;
            cancellation = null;
            settings.Enabled = fit.Enabled = true;
            stop.Enabled = false;
            if (closeWhenStopped) Close();
        }
    }
}
