using ScottPlot;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.Plottable;
using System.Globalization;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace BezierAirfoilDesigner
{
    public partial class Form1 : Form
    {
        readonly double minZoomRange = 0.01;
        readonly double maxZoomRange = 10.0;
        readonly List<PointF> defaultControlPointsTop = new()
        {
            new PointF(0, 0),
            new PointF(0, 0.15f),
            new PointF(0.5f, 0.15f),
            new PointF(1.0f, 0)
        };
        readonly List<PointF> defaultControlPointsBottom = new()
        {
            new PointF(0, 0),
            new PointF(0, -0.1f),
            new PointF(0.5f, -0.1f),
            new PointF(1.0f, 0)
        };
        List<PointF> referenceDatTop = new();
        List<PointF> referenceDatBottom = new();

        bool showControlTop;
        bool showControlBottom;
        bool showTop;
        bool showBottom;
        bool showThickness;
        bool showCamber;
        bool showRadius;
        bool showReferenceTop;
        bool showReferenceBottom;
        bool cancelAutoSearch;

        List<PointF> errorTop = new();
        List<PointF> errorBottom = new();
        float totalErrorTop;
        float totalErrorBottom;
        
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

                formsPlot1.SetBounds(14, 13, Form1.ActiveForm.Width - (1588 - 1085), Form1.ActiveForm.Height - (783 - 713));
                dataGridViewTop.SetBounds(Form1.ActiveForm.Width - (1588 - 1174), 31, 298, (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3);
                dataGridViewBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1174), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31, 298, (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3);

                lblBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1171), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 - 22, 55, 19);
                lblOrderBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1478), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31, 45, 19);
                btnIncreaseOrderBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1478), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 + 22, 26, 26);
                btnDecreaseOrderBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1513), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 + 22, 26, 26);
                lblNumOfPointBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1478), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 + 51, 75, 19);
                txtNumOfPointBottom.SetBounds(Form1.ActiveForm.Width - (1588 - 1478), (Form1.ActiveForm.Height - (783 - (783 - 31 - 25 - 394))) / 2 + 3 + 25 + 31 + 73, 86, 26);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FormsPlotSettings();
            GridViewSettings();
            AddToolTips();
            AddDefaultPointsTop();
            AddDefaultPointsBottom();

            chkShowControlTop.Checked = true;
            chkShowControlBottom.Checked = true;
            chkShowTop.Checked = true;
            chkShowBottom.Checked = true;

            chkShowReferenceTop.Enabled = false;
            chkShowReferenceBottom.Enabled = false;
            btnSearchTop.Enabled = false;
            btnSearchBottom.Enabled = false;
            btnAutoSearch.Enabled = false;

            calculations();
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }
        
        void calculations()
        {
            var axisLimits = formsPlot1.Plot.GetAxisLimits();
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.SetAxisLimits(axisLimits);

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating and plotting points on both bezier curves

            List<PointF> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            //try to parse the number of points, if unsucessful (eg not numeric) set to 3
            if (int.TryParse(txtNumOfPointsTop.Text, out int numPointsTop) == false || numPointsTop < 3)
            {
                txtNumOfPointsTop.Text = "3";
                numPointsTop = 3;
            }
            if (int.TryParse(txtNumOfPointBottom.Text, out int numPointsBottom) == false || numPointsBottom < 3)
            {
                txtNumOfPointBottom.Text = "3";
                numPointsBottom = 3;
            }

            List<PointF> pointsTop = DeCasteljau.BezierCurve(controlPointsTop, numPointsTop);
            List<PointF> pointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numPointsBottom);

            Color colorTop;
            if (showTop) { colorTop = Color.Red; } else { colorTop = Color.Transparent; }
            var top = formsPlot1.Plot.AddScatterList(color: colorTop, lineStyle: ScottPlot.LineStyle.Solid);
            Color colorBottom;
            if (showBottom) { colorBottom = Color.Red; } else { colorBottom = Color.Transparent; }
            var bottom = formsPlot1.Plot.AddScatterList(color: colorBottom, lineStyle: ScottPlot.LineStyle.Solid);

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
            // calculating and plotting the thickness line

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
            // calculating and plotting the camber line

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
            // calculating and drawing the LE radius

            PointF midpoint = CircleProperties.CalculateMidpoint(pointsBottom[1], pointsTop[0], pointsTop[1]);
            double radius = CircleProperties.CalculateRadius(pointsBottom[1], pointsTop[0], pointsTop[1]);
            Color circleColor;
            if (showRadius) { circleColor = Color.Gray; } else { circleColor = Color.Transparent; }
            formsPlot1.Plot.AddCircle(x: midpoint.X, y: midpoint.Y, radius: radius, color: circleColor, lineWidth: 1, lineStyle: ScottPlot.LineStyle.Dash);

            //----------------------------------------------------------------------------------------------------------------------------------
            // printing airfoil parameters to the text box

            txtAirfoilParam.Text = "";
            txtAirfoilParam.AppendText("nose radius:\t\t" + radius + System.Environment.NewLine);
            txtAirfoilParam.AppendText("maximum camber:\t" + maxCamber.Y.ToString() + "\t@: " + maxCamber.X.ToString() + System.Environment.NewLine);
            txtAirfoilParam.AppendText("maximum thickness:\t" + maxThickness.Y.ToString() + "\t@: " + maxThickness.X.ToString() + System.Environment.NewLine);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Plotting the control polygons of the top and bottom bezier curves

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
            if (showControlTop) { controlTop.Color = Color.Gray; } else { controlTop.Color = Color.Transparent; }
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
            if (showControlBottom) { controlBottom.Color = Color.Gray; } else { controlBottom.Color = Color.Transparent; }
            controlBottom.MarkerSize = 5;
            formsPlot1.Plot.Add(controlBottom);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Add the reference .dat airfoil to the plot


            Color referenceTopColor = new();
            if (showReferenceTop) { referenceTopColor = Color.Blue; } else { referenceTopColor = Color.Transparent; }
            var referenceDatPlotTop = formsPlot1.Plot.AddScatterList(color: referenceTopColor, lineStyle: ScottPlot.LineStyle.Dash, lineWidth: 1, markerSize: 4);

            Color referenceBottomColor = new();
            if (showReferenceBottom) { referenceBottomColor = Color.Blue; } else { referenceBottomColor = Color.Transparent; }
            var referenceDatPlotBottom = formsPlot1.Plot.AddScatterList(color: referenceBottomColor, lineStyle: ScottPlot.LineStyle.Dash, lineWidth: 1, markerSize: 4);

            for (int i = 0; i < referenceDatTop.Count; i++)
            {
                referenceDatPlotTop.Add(referenceDatTop[i].X, referenceDatTop[i].Y);
            }
            for (int i = 0; i < referenceDatBottom.Count; i++)
            {
                referenceDatPlotBottom.Add(referenceDatBottom[i].X, referenceDatBottom[i].Y);
            }

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating the error between the bezier airfoil and the reference airfoil and writing results to the text field

            errorTop.Clear();
            errorTop = GetThickness(pointsTop, referenceDatTop);
            totalErrorTop = CalculateAreaUnderCurve(errorTop) * 1000;

            errorBottom.Clear();
            errorBottom = GetThickness(pointsBottom, referenceDatBottom);
            totalErrorBottom = CalculateAreaUnderCurve(errorBottom) * 1000;

            if (totalErrorTop > 0) { txtAirfoilParam.AppendText("error top:\t\t" + totalErrorTop + System.Environment.NewLine); }
            if (totalErrorBottom > 0) { txtAirfoilParam.AppendText("error bottom:\t\t" + totalErrorBottom + System.Environment.NewLine); }

            //----------------------------------------------------------------------------------------------------------------------------------

            formsPlot1.Refresh();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // settings

        private void FormsPlotSettings()
        {
            formsPlot1.Plot.AxisScaleLock(enable: true, scaleMode: ScottPlot.EqualScaleMode.PreserveX);
            formsPlot1.Configuration.RightClickDragZoom = false;


        }
        private void GridViewSettings()
        {
            dataGridViewTop.AllowUserToResizeColumns = false;
            dataGridViewTop.AllowUserToResizeRows = false;
            dataGridViewTop.AllowUserToOrderColumns = false;

            foreach (DataGridViewColumn column in dataGridViewTop.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridViewBottom.AllowUserToResizeColumns = false;
            dataGridViewBottom.AllowUserToResizeRows = false;
            dataGridViewBottom.AllowUserToOrderColumns = false;

            foreach (DataGridViewColumn column in dataGridViewBottom.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void AddToolTips()
        {
            // Create a new instance of ToolTip
            ToolTip buttonToolTip = new()
            {
                // Set up some of the ToolTip settings
                AutoPopDelay = 5000,  // Time for which the ToolTip is shown
                InitialDelay = 500,  // Time delay when hovering over the control before the ToolTip is shown
                ReshowDelay = 500,    // Time delay when the mouse pointer is moved from one control to another
                ShowAlways = true    // Force the ToolTip text to be displayed whether or not the form is active
            };

            // Set up the ToolTip text
            buttonToolTip.SetToolTip(btnLoadDat, "right click to remove");
            buttonToolTip.SetToolTip(btnAutoSearch, "right click to cancel");
        }
        private void AddDefaultPointsTop()
        {
            dataGridViewTop.Rows.Clear();
            dataGridViewTop.Columns.Clear();
            dataGridViewTop.Columns.Add("xVal", "X");
            dataGridViewTop.Columns.Add("yVal", "Y");
            gridViewAddPoints(dataGridViewTop, defaultControlPointsTop);
        }
        private void AddDefaultPointsBottom()
        {
            dataGridViewBottom.Rows.Clear();
            dataGridViewBottom.Columns.Clear();
            dataGridViewBottom.Columns.Add("xVal", "X");
            dataGridViewBottom.Columns.Add("yVal", "Y");
            gridViewAddPoints(dataGridViewBottom, defaultControlPointsBottom);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // helper functions

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
                _ = float.TryParse(s: gridView.Rows[i].Cells[0].Value.ToString(), out float x);
                _ = float.TryParse(s: gridView.Rows[i].Cells[1].Value.ToString(), out float y);
                // Create a PointF object
                PointF point = new(x, y);
                controlPointsBottom.Add(point);
            }

            return controlPointsBottom;
        }
        private static List<PointF> GetThickness(List<PointF> curve1, List<PointF> curve2)
        {
            List<PointF> distances = new();

            if (!curve1.Any() || !curve2.Any()) { return distances; }

            // Ensure the points are sorted by X in ascending order.
            curve1 = curve1.OrderBy(p => p.X).ToList();
            curve2 = curve2.OrderBy(p => p.X).ToList();

            // The X range over which we calculate distances.
            float minX = Math.Max(curve1.First().X, curve2.First().X);
            float maxX = Math.Min(curve1.Last().X, curve2.Last().X);

            // Calculate vertical distances at regular intervals within this range.
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
            List<PointF> midpoints = new();
            for (float x = minX; x <= maxX; x += 0.001f)  // Adjust the step size as needed.
            {
                float? y1 = InterpolateY(x, curve1);
                float? y2 = InterpolateY(x, curve2);
                if (y1.HasValue && y2.HasValue)
                {
                    float lowerY = Math.Min(y1.Value, y2.Value);
                    float higherY = Math.Max(y1.Value, y2.Value);
                    float y = lowerY + 0.5f * (higherY - lowerY);
                    midpoints.Add(new PointF(x, y));
                }
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
        public static void SaveTextToFile(string text, string path)
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
        public static float CalculateAreaBetweenCurves(List<PointF> curve1Points, List<PointF> curve2Points)
        {
            float areaCurve1 = CalculateAreaUnderCurve(curve1Points);
            float areaCurve2 = CalculateAreaUnderCurve(curve2Points);

            return Math.Abs(areaCurve1 - areaCurve2);
        }
        public static float CalculateAreaUnderCurve(List<PointF> curvePoints)
        {
            float area = 0.0f;

            for (int i = 0; i < curvePoints.Count - 1; i++)
            {
                float h = curvePoints[i + 1].X - curvePoints[i].X;
                float avgY = (curvePoints[i].Y + curvePoints[i + 1].Y) / 2.0f;
                area += h * avgY;
            }

            return area;
        }
        
        //--------------------------------------------------------------------------------------------------------------------------------------
        // Form events
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            if (!controlPointsTop.SequenceEqual(defaultControlPointsTop) ||
                !controlPointsBottom.SequenceEqual(defaultControlPointsBottom))
            {
                var result = MessageBox.Show("There are unsaved changes. Do you really want to close?", "Unsaved changes", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // formsPlot events

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
            else if (Math.Abs(limits.XMax - limits.XMin) > maxZoomRange)
            {
                // Calculate the center of the current x range
                double xCenter = (limits.XMin + limits.XMax) / 2;

                // Set new x limits that maintain the center but decrease the range
                double xMin = xCenter - maxZoomRange / 2;
                double xMax = xCenter + maxZoomRange / 2;

                // Set the axis limits to the adjusted values
                formsPlot1.Plot.SetAxisLimits(xMin, xMax, limits.YMin, limits.YMax);
            }

            // Render the plot
            formsPlot1.Plot.Render();
        }
        private void formsPlot1_PlottableDragged(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            (double mouseCoordX, double mouseCoordY) = formsPlot1.GetMouseCoordinates();
            txtAirfoilParam.Text = $"Mouse coords ({mouseCoordX:N8}, {mouseCoordY:N8})" + System.Environment.NewLine;
            PointF mouse = new(float.Parse(mouseCoordX.ToString()), float.Parse(mouseCoordY.ToString()));

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

            if (indexLowestDistance > 0) //protect against dragging the LE point
            {
                if (topOrBottom && indexLowestDistance < controlPointsTop.Count - 1) //chose top or bottom and protect against dragging the TE point
                {
                    controlPointsTop[indexLowestDistance] = mouse;
                    gridViewAddPoints(dataGridViewTop, controlPointsTop);
                }
                else if (!topOrBottom && indexLowestDistance < controlPointsBottom.Count - 1) //chose top or bottom and protect against dragging the TE point
                {
                    controlPointsBottom[indexLowestDistance] = mouse;
                    gridViewAddPoints(dataGridViewBottom, controlPointsBottom);
                }
            }


            calculations();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // dataGridView events

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridViewTop.Rows.Count - 1; i++)
            {
                if (dataGridViewTop.Rows[i].Cells[0].Value == null) { dataGridViewTop.Rows[i].Cells[0].Value = 0.0f; }
                if (dataGridViewTop.Rows[i].Cells[1].Value == null) { dataGridViewTop.Rows[i].Cells[1].Value = 0.0f; }
            }
            calculations();
        }
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridViewBottom.Rows.Count - 1; i++)
            {
                if (dataGridViewBottom.Rows[i].Cells[0].Value == null) { dataGridViewBottom.Rows[i].Cells[0].Value = 0.0f; }
                if (dataGridViewBottom.Rows[i].Cells[1].Value == null) { dataGridViewBottom.Rows[i].Cells[1].Value = 0.0f; }
            }
            calculations();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // checkbox events

        private void chkShowControlTop_CheckedChanged(object sender, EventArgs e)
        {
            showControlTop = chkShowControlTop.Checked;
            calculations();
        }
        private void chkShowControlBottom_CheckedChanged(object sender, EventArgs e)
        {
            showControlBottom = chkShowControlBottom.Checked;
            calculations();
        }
        private void chkShowTop_CheckedChanged(object sender, EventArgs e)
        {
            showTop = chkShowTop.Checked;
            calculations();
        }
        private void chkShowBottom_CheckedChanged(object sender, EventArgs e)
        {
            showBottom = chkShowBottom.Checked;
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
        private void chkShowReferenceTop_CheckedChanged(object sender, EventArgs e)
        {
            showReferenceTop = chkShowReferenceTop.Checked;
            calculations();
        }
        private void chkShowReferenceBottom_CheckedChanged(object sender, EventArgs e)
        {
            showReferenceBottom = chkShowReferenceBottom.Checked;
            calculations();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // button events

        private void btnDefault_Click(object sender, EventArgs e)
        {
            AddDefaultPointsTop();
            AddDefaultPointsBottom();
            calculations();
        }
        private void btnIncreaseOrderTop_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridViewTop);
            controlPointsTop = DeCasteljau.IncreaseOrder(controlPointsTop);
            gridViewAddPoints(dataGridViewTop, controlPointsTop);
            calculations();
        }
        private void btnIncreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsBottom = GetControlPoints(dataGridViewBottom);
            controlPointsBottom = DeCasteljau.IncreaseOrder(controlPointsBottom);
            gridViewAddPoints(dataGridViewBottom, controlPointsBottom);
            calculations();
        }
        private void btnDecreaseOrderTop_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridViewTop);
            controlPointsTop = DeCasteljau.DecreaseOrder(controlPointsTop);
            gridViewAddPoints(dataGridViewTop, controlPointsTop);
            calculations();
        }
        private void btnDecreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsBottom = GetControlPoints(dataGridViewBottom);
            controlPointsBottom = DeCasteljau.DecreaseOrder(controlPointsBottom);
            gridViewAddPoints(dataGridViewBottom, controlPointsBottom);
            calculations();
        }
        private void btnSaveDat_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            if (int.Parse(txtNumOfPointsTop.Text) < 3) { txtNumOfPointsTop.Text = "3"; }
            if (int.Parse(txtNumOfPointBottom.Text) < 3) { txtNumOfPointBottom.Text = "3"; }

            int numPointsTop = int.Parse(txtNumOfPointsTop.Text);
            int numPointsBottom = int.Parse(txtNumOfPointBottom.Text);

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



            using SaveFileDialog sfd = new();
            sfd.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveTextToFile(datFile, sfd.FileName);
            }
        }
        private void btnSaveBezDat_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridViewBottom);

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

            using SaveFileDialog sfd = new();
            sfd.Filter = "Bezier dat files (*.bez.dat)|*.bez.dat|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveTextToFile(datFile, sfd.FileName);
            }
        }
        private void btnAxisAuto_Click(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }
        private void btnLoadDat_Click(object sender, EventArgs e)
        {
            referenceDatTop.Clear();
            referenceDatBottom.Clear();

            List<PointF> points = new();

            // Create a new instance of the OpenFileDialog class
            OpenFileDialog openFileDialog = new()
            {
                // Set some properties to define how the dialog works
                //InitialDirectory = "c:\\", // Starting directory
                Filter = "Dat files (*.dat)|*.dat" // Only show .dat files
            };

            // Show the dialog and get the result
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // The user selected a file and clicked OK, so the FileName property now contains the selected file path
                string path = openFileDialog.FileName;

                using StreamReader reader = new(path); // Use the path chosen by the user
                                                       // skip the header line
                string line = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    // split the line on whitespace
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 2)
                    {
                        // parse the parts as floats
                        if (float.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float x)
                        && float.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float y))
                        {
                            // add the point to the list
                            points.Add(new PointF(x, y));
                        }
                    }
                }
            }
            else { return; } // The user did not select a file and clicked Cancel, so exit the method

            int index = 0;
            float minumum = float.PositiveInfinity;

            for (int i = 1; i < points.Count; i++)
            {
                if (Math.Abs(points[i].X) + Math.Abs(points[i].Y) < minumum)
                {
                    minumum = Math.Abs(points[i].X) + Math.Abs(points[i].Y);
                    index = i;
                }
            }

            referenceDatTop = points.GetRange(0, index + 1);  // From start to minimum LE point has to be duplicated in split
            referenceDatTop.Reverse();

            referenceDatBottom = points.GetRange(index, points.Count - index);  // From minimum to end

            chkShowReferenceTop.Enabled = true;
            chkShowReferenceBottom.Enabled = true;
            chkShowReferenceTop.Checked = true;
            chkShowReferenceBottom.Checked = true;

            btnSearchTop.Enabled = true;
            btnSearchBottom.Enabled = true;
            btnAutoSearch.Enabled = true;

            calculations();
        }
        private void btnLoadBezDat_Click(object sender, EventArgs e)
        {
            List<PointF> controlPoints = new();

            // Create a new instance of the OpenFileDialog class
            OpenFileDialog openFileDialog = new()
            {
                // Set some properties to define how the dialog works
                //InitialDirectory = "c:\\", // Starting directory
                Filter = "Bezier dat files (*.bez.dat)|*.bez.dat" // Only show .bez.dat files
            };

            // Show the dialog and get the result
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // The user selected a file and clicked OK, so the FileName property now contains the selected file path
                string path = openFileDialog.FileName;

                using StreamReader reader = new(path); // Use the path chosen by the user
                                                       // skip the header line
                string line = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    // split the line on whitespace
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 2)
                    {
                        // parse the parts as floats
                        if (float.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float x)
                        && float.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float y))
                        {
                            // add the point to the list
                            controlPoints.Add(new PointF(x, y));
                        }
                    }
                }
            }
            else { return; } // The user did not select a file and clicked Cancel, so exit the method

            int index = 0;

            for (int i = 1; i < controlPoints.Count; i++)
            {
                if (Math.Abs(controlPoints[i].X) + Math.Abs(controlPoints[i].Y) < Math.Abs(controlPoints[i - 1].X) + Math.Abs(controlPoints[i - 1].Y)) { index = i; }
            }

            List<PointF> controlPointsTop = controlPoints.GetRange(0, index + 1);  // From start to minimum (inclusive)
            controlPointsTop.Reverse(); //control Points are stored just like in a .dat from TE over the top to LE under the bottom to TE
                                        //LE point has to be duplicated in split

            List<PointF> controlPointsBottom = controlPoints.GetRange(index, controlPoints.Count - index);  // From minimum (exclusive) to end

            gridViewAddPoints(dataGridViewTop, controlPointsTop);
            gridViewAddPoints(dataGridViewBottom, controlPointsBottom);

            calculations();
        }
        private void btnLoadDat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // The right mouse button was pressed
                referenceDatTop.Clear();
                referenceDatBottom.Clear();

                chkShowReferenceTop.Enabled = false;
                chkShowReferenceBottom.Enabled = false;
                chkShowReferenceTop.Checked = false;
                chkShowReferenceBottom.Checked = false;

                btnSearchTop.Enabled = false;
                btnSearchBottom.Enabled = false;
                btnAutoSearch.Enabled = false;
            }

            calculations();
        }
        private void btnSearchTop_Click(object sender, EventArgs e)
        {
            List<PointF> controlPoints = GetControlPoints(dataGridViewTop);
            SearchControlPoints(controlPoints, dataGridViewTop);

            Console.Beep();
        }
        private void btnSearchBottom_Click(object sender, EventArgs e)
        {
            List<PointF> controlPoints = GetControlPoints(dataGridViewBottom);
            SearchControlPoints(controlPoints, dataGridViewBottom);

            Console.Beep();
        }
        private void SearchControlPoints(List<PointF> controlPoints, DataGridView gridView)
        {
            float currentLowestError = (gridView == dataGridViewTop) ? totalErrorTop : totalErrorBottom;
            List<PointF> controlPointsWithLowestError = new(controlPoints); // Store initial state
            int numPoints = 3; // Start with searching two points: current point and one extra point

            bool betterCombinationFound = false;

            while (!betterCombinationFound)
            {
                float searchDistance = currentLowestError / 100; // Total search distance
                float searchStep = searchDistance / (numPoints - 1); // Defines the step size

                SearchCombinations(controlPoints, 1, searchStep, numPoints);

                void SearchCombinations(List<PointF> points, int currentIndex, float step, int numSteps)
                {
                    if (currentIndex >= points.Count - 1)
                    {
                        gridViewAddPoints(gridView, points);
                        calculations();
                        float currentError = (gridView == dataGridViewTop) ? totalErrorTop : totalErrorBottom;
                        if (currentError < currentLowestError)
                        {
                            currentLowestError = currentError;
                            controlPointsWithLowestError.Clear();
                            controlPointsWithLowestError.AddRange(points);
                            betterCombinationFound = true;
                        }
                        return;
                    }

                    PointF originalPoint = points[currentIndex];
                    float searchMin = originalPoint.Y - searchDistance / 2; // Start from below the current Y-coordinate

                    // Iterates over numPoints specific points around the current Y-coordinate
                    for (int i = 0; i < numSteps; i++)
                    {
                        float y = searchMin + i * step;
                        points[currentIndex] = new PointF(originalPoint.X, y);
                        SearchCombinations(points, currentIndex + 1, step, numSteps);
                    }

                    points[currentIndex] = originalPoint; // Restore original point before returning.
                }

                Console.Beep();
                numPoints++; // Increase number of points for next iteration
            }

            //if (controlPointsWithLowestError.Count > 0)
            //{
            gridViewAddPoints(gridView, controlPointsWithLowestError);
            calculations();
            //}
        }
        private void btnAutoSearch_Click(object sender, EventArgs e)
        {
            // Record the start time
            DateTime startTime = DateTime.Now;

            // Define thresholds
            float errorThresholdTop = 0.075f;
            float errorThresholdBottom = 0.075f;
            float improvementThreshold = 0.05f;

            bool increaseOrderFlag = false;

            while (totalErrorTop > errorThresholdTop || totalErrorBottom > errorThresholdBottom)
            {
                // Check if the operation should be cancelled
                if (cancelAutoSearch)
                    break;

                float previousErrorTop = totalErrorTop;
                float previousErrorBottom = totalErrorBottom;
                float currentErrorTop;
                float currentErrorBottom;
                float errorImprovementTop = 1.0f;
                float errorImprovementBottom = 1.0f;

                if (increaseOrderFlag)
                {
                    btnIncreaseOrderTop.PerformClick();
                    btnIncreaseOrderBottom.PerformClick();
                    increaseOrderFlag = false;
                }

                while ((totalErrorTop > errorThresholdTop && errorImprovementTop >= improvementThreshold) ||
                       (totalErrorBottom > errorThresholdBottom && errorImprovementBottom >= improvementThreshold))
                {
                    // Check if the operation should be cancelled
                    if (cancelAutoSearch)
                        break;

                    if (totalErrorTop > errorThresholdTop)
                    {
                        btnSearchTop.PerformClick();
                        currentErrorTop = totalErrorTop;
                        errorImprovementTop = (previousErrorTop - currentErrorTop) / previousErrorTop;
                        previousErrorTop = currentErrorTop;
                    }

                    if (totalErrorBottom > errorThresholdBottom)
                    {
                        btnSearchBottom.PerformClick();
                        currentErrorBottom = totalErrorBottom;
                        errorImprovementBottom = (previousErrorBottom - currentErrorBottom) / previousErrorBottom;
                        previousErrorBottom = currentErrorBottom;
                    }
                }

                increaseOrderFlag = true;
            }

            // Record the end time and calculate elapsed time
            DateTime endTime = DateTime.Now;
            TimeSpan elapsedTime = endTime - startTime;

            // If operation wasn't cancelled, play a beep and show a message box with start time, end time and elapsed time
            if (!cancelAutoSearch)
            {
                // Play a beep
                Console.Beep();
                Console.Beep();
                Console.Beep();

                // Show a message box with start time, end time and elapsed time
                MessageBox.Show($"Start time: {startTime}\nEnd time: {endTime}\nElapsed time: {elapsedTime}");
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}