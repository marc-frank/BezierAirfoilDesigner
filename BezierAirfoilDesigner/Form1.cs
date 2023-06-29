//using MathNet.Numerics.LinearAlgebra;
//using MathNet.Numerics.LinearAlgebra.Double;
using ScottPlot;
using ScottPlot.Plottable;
using System.Windows.Forms;
//using System.Numerics;

namespace BezierAirfoilDesigner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Form1.ActiveForm != null)
            {
                //if (Form1.ActiveForm.Width < 1588) { Form1.ActiveForm.Width = 1588; }
                //if (Form1.ActiveForm.Height < 783) { Form1.ActiveForm.Height = 783; }

                formsPlot1.SetBounds(14, 13, Form1.ActiveForm.Width - (1588 - 1152), Form1.ActiveForm.Height - (783 - 713));
                dataGridView1.SetBounds(Form1.ActiveForm.Width - (1588 - 1174), 31, 298, (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3);
                dataGridView2.SetBounds(Form1.ActiveForm.Width - (1588 - 1174), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31, 298, (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3);

                label2.SetBounds(Form1.ActiveForm.Width - (1588 - 1171), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 - 22, 55, 19);
                lblOrderBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1478), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31, 45, 19);
                btnIncreaseOrderBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1478), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 + 22, 26, 26);
                btnDecreaseOrderBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1513), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 + 22, 26, 26);
                label5.SetBounds(Form1.ActiveForm.Width - (1588 - 1478), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 + 51, 75, 19);
                textBox2.SetBounds(Form1.ActiveForm.Width - (1588 - 1478), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 + 73, 86, 26);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormsPlotSettings();
            GridViewSettings();
            AddDefaultPointsTop();
            AddDefaultPointsBottom();

            chkShowControlPolygon.Checked = true;
            chkShowThickness.Checked = false;
            chkShowCamber.Checked = false;
            chkShowRadius.Checked = false;

            calculations();
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }

        double minZoomRange = 0.01;
        bool showControlPolygon;
        bool showThickness;
        bool showCamber;
        bool showRadius;

        void calculations()
        {
            var axisLimits = formsPlot1.Plot.GetAxisLimits();
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.SetAxisLimits(axisLimits);

            //----------------------------------------------------------------------------------------------------------------------------------

            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);

            if (int.Parse(textBox1.Text) < 3) { textBox1.Text = "3"; }
            if (int.Parse(textBox2.Text) < 3) { textBox2.Text = "3"; }

            int numPointsTop = int.Parse(textBox1.Text);
            int numPointsBottom = int.Parse(textBox2.Text);

            List<PointF> pointsTop = DeCasteljau.BezierCurve(controlPointsTop, numPointsTop);
            List<PointF> pointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numPointsBottom);

            var top = formsPlot1.Plot.AddScatterList(color: Color.Red, lineStyle: ScottPlot.LineStyle.Solid);
            var bottom = formsPlot1.Plot.AddScatterList(color: Color.Red, lineStyle: ScottPlot.LineStyle.Solid);

            for (int i = 0; i < pointsTop.Count; i++)
            {
                top.Add(pointsTop[i].X, pointsTop[i].Y);
            }
            for (int i = 0; i < pointsBottom.Count; i++)
            {
                bottom.Add(pointsBottom[i].X, pointsBottom[i].Y);
            }

            lblOrderTop.Text = "order: " + (controlPointsTop.Count - 1).ToString();
            lblOrderBottom.Text = "order: " + (controlPointsBottom.Count - 1).ToString();

            //----------------------------------------------------------------------------------------------------------------------------------

            Color thicknessLineColor = new();
            if (showThickness) { thicknessLineColor = Color.Gray; } else { thicknessLineColor = Color.Transparent; }
            var thicknessLine = formsPlot1.Plot.AddScatterList(color: thicknessLineColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);

            List<PointF> thicknesses = GetThickness(pointsTop, pointsBottom);
            PointF maxThickness = new();

            for (int i = 0; i < thicknesses.Count; i++)
            {
                thicknessLine.Add(thicknesses[i].X, thicknesses[i].Y);
                if (thicknesses[i].Y > maxThickness.Y)
                {
                    maxThickness = thicknesses[i];
                }
            }

            Color maxThicknessMarkColor = new();
            if (showThickness) { maxThicknessMarkColor = Color.Gray; } else { maxThicknessMarkColor = Color.Transparent; }
            var maxThicknessMark = formsPlot1.Plot.AddScatterList(color: maxThicknessMarkColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);
            maxThicknessMark.Add(maxThickness.X, maxThickness.Y);
            maxThicknessMark.Add(maxThickness.X, 0.0f);

            //----------------------------------------------------------------------------------------------------------------------------------

            Color midLineColor = new();
            if (showCamber) { midLineColor = Color.Gray; } else { midLineColor = Color.Transparent; }
            var camberLine = formsPlot1.Plot.AddScatterList(color: midLineColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);

            List<PointF> camber = GetCamber(pointsTop, pointsBottom);
            PointF maxCamber = new();

            for (int i = 0; i < camber.Count; i++)
            {
                camberLine.Add(camber[i].X, camber[i].Y);
                if (Math.Abs(camber[i].Y) > Math.Abs(maxCamber.Y))
                {
                    maxCamber = camber[i];
                }
            }

            Color maxCamberMarkColor = new();
            if (showCamber) { maxCamberMarkColor = Color.Gray; } else { maxCamberMarkColor = Color.Transparent; }
            var maxCamberMark = formsPlot1.Plot.AddScatterList(color: maxCamberMarkColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);
            maxCamberMark.Add(maxCamber.X, maxCamber.Y);
            maxCamberMark.Add(maxCamber.X, 0.0f);

            //----------------------------------------------------------------------------------------------------------------------------------

            PointF midpoint = CircleProperties.CalculateMidpoint(pointsBottom[1], pointsTop[0], pointsTop[1]);
            double radius = CircleProperties.CalculateRadius(pointsBottom[1], pointsTop[0], pointsTop[1]);
            Color circleColor;
            if (showRadius) { circleColor = Color.Gray; } else { circleColor = Color.Transparent; }
            formsPlot1.Plot.AddCircle(x: midpoint.X, y: midpoint.Y, radius: radius, color: circleColor, lineWidth: 1, lineStyle: ScottPlot.LineStyle.Dash);

            //----------------------------------------------------------------------------------------------------------------------------------

            richTextBox2.Text = "";
            richTextBox2.AppendText("nose radius: " + radius + System.Environment.NewLine);
            richTextBox2.AppendText("maximum camber: " + maxCamber.Y.ToString() + " @: " + maxCamber.X.ToString() + System.Environment.NewLine);
            richTextBox2.AppendText("maximum thickness: " + maxThickness.Y.ToString() + " @: " + maxThickness.X.ToString() + System.Environment.NewLine);

            //----------------------------------------------------------------------------------------------------------------------------------

            double[] xT = new double[controlPointsTop.Count];
            double[] yT = new double[controlPointsTop.Count];

            for (int i = 0; i < controlPointsTop.Count; i++)
            {
                xT[i] = controlPointsTop[i].X;
                yT[i] = controlPointsTop[i].Y;
            }

            var controlTop = new ScottPlot.Plottable.ScatterPlotListDraggable();
            controlTop.AddRange(xT, yT);
            controlTop.LineStyle = LineStyle.Dash;
            if (showControlPolygon) { controlTop.Color = Color.Gray; } else { controlTop.Color = Color.Transparent; }
            controlTop.MarkerSize = 5;
            formsPlot1.Plot.Add(controlTop);

            double[] xB = new double[controlPointsBottom.Count];
            double[] yB = new double[controlPointsBottom.Count];

            for (int i = 0; i < controlPointsBottom.Count; i++)
            {
                xB[i] = controlPointsBottom[i].X;
                yB[i] = controlPointsBottom[i].Y;
            }

            var controlBottom = new ScottPlot.Plottable.ScatterPlotListDraggable();
            controlBottom.AddRange(xB, yB);
            controlBottom.LineStyle = LineStyle.Dash;
            if (showControlPolygon) { controlBottom.Color = Color.Gray; } else { controlBottom.Color = Color.Transparent; }
            controlBottom.MarkerSize = 5;
            formsPlot1.Plot.Add(controlBottom);

            //----------------------------------------------------------------------------------------------------------------------------------

            formsPlot1.Refresh();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private void FormsPlotSettings()
        {
            formsPlot1.Plot.AxisScaleLock(enable: true, scaleMode: ScottPlot.EqualScaleMode.PreserveX);
            formsPlot1.Configuration.RightClickDragZoom = false;


        }
        private void GridViewSettings()
        {
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToOrderColumns = false;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.AllowUserToOrderColumns = false;

            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void AddDefaultPointsTop()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("xVal", "X");
            dataGridView1.Columns.Add("yVal", "Y");
            dataGridView1.Rows.Add(0, 0);
            dataGridView1.Rows.Add(0, 0.15);
            dataGridView1.Rows.Add(0.5, 0.15);
            dataGridView1.Rows.Add(1.0, 0.0);
        }
        private void AddDefaultPointsBottom()
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.Columns.Add("xVal", "X");
            dataGridView2.Columns.Add("yVal", "Y");
            dataGridView2.Rows.Add(0, 0);
            dataGridView2.Rows.Add(0, -0.1);
            dataGridView2.Rows.Add(0.5, -0.1);
            dataGridView2.Rows.Add(1.0, 0.0);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private static void gridViewAddPoints(DataGridView dataGridView, List<PointF> pointFs)
        {
            dataGridView.Rows.Clear();
            //dataGridView.Columns.Clear();
            //dataGridView.Columns.Add("xVal", "X");
            //dataGridView.Columns.Add("yVal", "Y");
            for (int i = 0; i < pointFs.Count; i++)
            {
                dataGridView.Rows.Add(pointFs[i].X, pointFs[i].Y);
            }
        }
        private static float GetDistanceBetweenPoints(PointF pointA, PointF pointB)
        {
            double distanceX = pointA.X - pointB.X;
            double distanceY = pointA.Y - pointB.Y;
            float distance = float.Parse((Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2))).ToString());
            return distance;
        }
        private static List<PointF> GetControlPoints(DataGridView gridView)
        {
            List<PointF> controlPointsBottom = new();

            for (int i = 0; i < gridView.Rows.Count - 1; i++)
            {
                // Retrieve the values from the DataGridView
                float x, y;
                _ = float.TryParse(s: gridView.Rows[i].Cells[0].Value.ToString(), out x);
                _ = float.TryParse(s: gridView.Rows[i].Cells[1].Value.ToString(), out y);
                // Create a PointF object
                PointF point = new(x, y);
                controlPointsBottom.Add(point);
            }

            return controlPointsBottom;
        }
        private static List<PointF> GetThickness(List<PointF> curve1, List<PointF> curve2)
        {
            // Ensure the points are sorted by X in ascending order.
            curve1 = curve1.OrderBy(p => p.X).ToList();
            curve2 = curve2.OrderBy(p => p.X).ToList();

            // The X range over which we calculate distances.
            float minX = Math.Max(curve1.First().X, curve2.First().X);
            float maxX = Math.Min(curve1.Last().X, curve2.Last().X);

            // Calculate vertical distances at regular intervals within this range.
            List<PointF> distances = new List<PointF>();
            for (float x = minX; x <= maxX; x += 0.001f)  // Adjust the step size as needed.
            {
                float? y1 = InterpolateY(x, curve1);
                float? y2 = InterpolateY(x, curve2);
                if (y1.HasValue && y2.HasValue)
                    distances.Add(new PointF(x, Math.Abs(y1.Value - y2.Value)));
            }

            // Now `distances` contains the vertical distances between the curves at regular intervals.

            return distances;
        }
        private static List<PointF> GetCamber(List<PointF> curve1, List<PointF> curve2)
        {
            // Ensure the points are sorted by X in ascending order.
            curve1 = curve1.OrderBy(p => p.X).ToList();
            curve2 = curve2.OrderBy(p => p.X).ToList();

            // The X range over which we calculate distances.
            float minX = Math.Max(curve1.First().X, curve2.First().X);
            float maxX = Math.Min(curve1.Last().X, curve2.Last().X);

            // Calculate midpoints at regular intervals within this range.
            List<PointF> midpoints = new List<PointF>();
            for (float x = minX; x <= maxX; x += 0.001f)  // Adjust the step size as needed.
            {
                float? y1 = InterpolateY(x, curve1);
                float? y2 = InterpolateY(x, curve2);
                if (y1.HasValue && y2.HasValue)
                    midpoints.Add(new PointF(x, (y1.Value + y2.Value) / 2));
            }

            // Now `midpoints` contains the midpoints between the curves at regular intervals,
            // stored as PointF where X is the x-value and Y is the midpoint Y value.

            return midpoints;
        }
        static float? InterpolateY(float x, List<PointF> curve)
        {
            for (int i = 0; i < curve.Count - 1; i++)
            {
                if (curve[i].X <= x && x <= curve[i + 1].X)
                    return curve[i].Y + (x - curve[i].X) / (curve[i + 1].X - curve[i].X) * (curve[i + 1].Y - curve[i].Y);
            }
            return null;  // X is outside the range of the curve.
        }
        public void SaveTextToFile(string text, string path)
        {
            try
            {
                File.WriteAllText(path, text);
                MessageBox.Show("File Saved Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private void formsPlot1_AxesChanged(object sender, EventArgs e)
        {
            // Get the current axis limits
            var limits = formsPlot1.Plot.GetAxisLimits();

            // Check if the x range is below the minimum
            if (Math.Abs(limits.XMax - limits.XMin) < minZoomRange)
            {
                // Calculate the center of the current x range
                double xCenter = (limits.XMin + limits.XMax) / 2;

                // Set new x limits that maintain the center but increase the range
                double xMin = xCenter - minZoomRange / 2;
                double xMax = xCenter + minZoomRange / 2;

                // Set the axis limits to the adjusted values
                formsPlot1.Plot.SetAxisLimits(xMin, xMax, limits.YMin, limits.YMax);
            }

            // Render the plot
            formsPlot1.Plot.Render();
        }
        private void formsPlot1_PlottableDragged(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);

            (double mouseCoordX, double mouseCoordY) = formsPlot1.GetMouseCoordinates();
            richTextBox2.Text = $"Mouse coords ({mouseCoordX:N8}, {mouseCoordY:N8})" + System.Environment.NewLine;
            PointF mouse = new PointF(float.Parse(mouseCoordX.ToString()), float.Parse(mouseCoordY.ToString()));

            float lowestDistanceTop = float.PositiveInfinity;
            int indexLowestDistanceTop = 0;
            float lowestDistanceBottom = float.PositiveInfinity;
            int indexLowestDistanceBottom = 0;

            float lowestDistance = float.PositiveInfinity;
            int indexLowestDistance = 0;
            bool topOrBottom;

            for (int i = 0; i < controlPointsTop.Count; i++)
            {
                float currentDistance = GetDistanceBetweenPoints(mouse, controlPointsTop[i]);
                if (currentDistance < lowestDistanceTop)
                {
                    lowestDistanceTop = currentDistance;
                    indexLowestDistanceTop = i;
                }
            }
            for (int i = 0; i < controlPointsBottom.Count; i++)
            {
                float currentDistance = GetDistanceBetweenPoints(controlPointsBottom[i], mouse);
                if (currentDistance < lowestDistanceBottom)
                {
                    lowestDistanceBottom = currentDistance;
                    indexLowestDistanceBottom = i;
                }
            }

            if (lowestDistanceTop < lowestDistanceBottom)
            {
                lowestDistance = lowestDistanceTop;
                indexLowestDistance = indexLowestDistanceTop;
                topOrBottom = true;
            }
            else
            {
                lowestDistance = lowestDistanceBottom;
                indexLowestDistance = indexLowestDistanceBottom;
                topOrBottom = false;
            }

            richTextBox2.AppendText(lowestDistance.ToString() + System.Environment.NewLine);
            richTextBox2.AppendText(indexLowestDistance.ToString() + System.Environment.NewLine);
            richTextBox2.AppendText(topOrBottom.ToString() + System.Environment.NewLine);

            if (topOrBottom)
            {
                controlPointsTop[indexLowestDistance] = mouse;
                gridViewAddPoints(dataGridView1, controlPointsTop);
            }
            else
            {
                controlPointsBottom[indexLowestDistance] = mouse;
                gridViewAddPoints(dataGridView2, controlPointsBottom);
            }

            calculations();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value == null) { dataGridView1.Rows[i].Cells[0].Value = 0.0f; }
                if (dataGridView1.Rows[i].Cells[1].Value == null) { dataGridView1.Rows[i].Cells[1].Value = 0.0f; }
            }
            calculations();
        }
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                if (dataGridView2.Rows[i].Cells[0].Value == null) { dataGridView2.Rows[i].Cells[0].Value = 0.0f; }
                if (dataGridView2.Rows[i].Cells[1].Value == null) { dataGridView2.Rows[i].Cells[1].Value = 0.0f; }
            }
            calculations();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private void chkShowControlPolygon_CheckedChanged(object sender, EventArgs e)
        {
            showControlPolygon = chkShowControlPolygon.Checked;
            calculations();
        }
        private void chkShowRadius_CheckedChanged(object sender, EventArgs e)
        {
            showRadius = chkShowRadius.Checked;
            calculations();
        }
        private void chkShowThickness_CheckedChanged(object sender, EventArgs e)
        {
            showThickness = chkShowThickness.Checked;
            calculations();
        }
        private void chkShowCamber_CheckedChanged(object sender, EventArgs e)
        {
            showCamber = chkShowCamber.Checked;
            calculations();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private void btnDefault_Click(object sender, EventArgs e)
        {
            AddDefaultPointsTop();
            AddDefaultPointsBottom();
            calculations();
        }
        private void btnIncreaseOrderTop_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);
            controlPointsTop = DeCasteljau.IncreaseOrder(controlPointsTop);
            gridViewAddPoints(dataGridView1, controlPointsTop);
            calculations();
        }
        private void btnIncreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);
            controlPointsBottom = DeCasteljau.IncreaseOrder(controlPointsBottom);
            gridViewAddPoints(dataGridView2, controlPointsBottom);
            calculations();
        }
        private void btnDecreaseOrderTop_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);
            controlPointsTop = DeCasteljau.DecreaseOrder(controlPointsTop);
            gridViewAddPoints(dataGridView1, controlPointsTop);
            calculations();
        }
        private void btnDecreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);
            controlPointsBottom = DeCasteljau.DecreaseOrder(controlPointsBottom);
            gridViewAddPoints(dataGridView2, controlPointsBottom);
            calculations();
        }
        private void btnSaveDat_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);

            if (int.Parse(textBox1.Text) < 3) { textBox1.Text = "3"; }
            if (int.Parse(textBox2.Text) < 3) { textBox2.Text = "3"; }

            int numPointsTop = int.Parse(textBox1.Text);
            int numPointsBottom = int.Parse(textBox2.Text);

            List<PointF> pointsTop = DeCasteljau.BezierCurve(controlPointsTop, numPointsTop);
            List<PointF> pointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numPointsBottom);

            string datFile = "" + "Airfoil Name" + System.Environment.NewLine;

            for (int i = pointsTop.Count - 1; i >= 0; i--)
            {
                datFile += ($"{pointsTop[i].X:N8} {pointsTop[i].Y:N8}" + System.Environment.NewLine);
            }

            for (int i = 1; i <= pointsBottom.Count - 1; i++)
            {
                datFile += ($"{pointsBottom[i].X:N8} {pointsBottom[i].Y:N8}" + System.Environment.NewLine);
            }

            datFile = datFile.Replace(',', '.');



            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SaveTextToFile(datFile, sfd.FileName);
                }
            }
        }
        private void btnSaveBezDat_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);

            string datFile = "" + "Airfoil Name" + System.Environment.NewLine;

            for (int i = controlPointsTop.Count - 1; i >= 0; i--)
            {
                datFile += ($"{controlPointsTop[i].X:N8} {controlPointsTop[i].Y:N8}" + System.Environment.NewLine);
            }

            for (int i = 1; i <= controlPointsBottom.Count - 1; i++)
            {
                datFile += ($"{controlPointsBottom[i].X:N8} {controlPointsBottom[i].Y:N8}" + System.Environment.NewLine);
            }

            datFile = datFile.Replace(',', '.');



            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Bezier dat files (*.bez.dat)|*.bez.dat|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SaveTextToFile(datFile, sfd.FileName);
                }
            }
        }
        private void btnAxisAuto_Click(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}