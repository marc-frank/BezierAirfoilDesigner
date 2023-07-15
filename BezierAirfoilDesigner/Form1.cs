using ScottPlot;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.Plottable;
using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        bool cancelSearch;

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

                formsPlot1.SetBounds(14, 13, Form1.ActiveForm.Width - (1588 - 1085), Form1.ActiveForm.Height - (783 - 684));
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

            //progressBar1.Visible = false;

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

            //try to parse the number of points, if unsucessful(eg not numeric) set to 3
            if (int.TryParse(txtNumOfPointsTop.Text, out int numPointsTop) == false || numPointsTop < 2 || numPointsTop > 100000)
            {
                txtNumOfPointsTop.Text = "225";
                numPointsTop = 225;
            }
            if (int.TryParse(txtNumOfPointBottom.Text, out int numPointsBottom) == false || numPointsBottom < 2 || numPointsBottom > 100000)
            {
                txtNumOfPointBottom.Text = "225";
                numPointsBottom = 225;
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

            // Try to parse the thickness step size, if unsuccessful (e.g. not numeric) set to 0.001f
            if (float.TryParse(txtThicknessStepSize.Text, out float thicknessStepSize) == false || thicknessStepSize < 0.00001f || thicknessStepSize > 1.0f)
            {
                txtThicknessStepSize.Text = "0,001";
                thicknessStepSize = 0.001f;
            }

            List<PointF> thicknesses = GetThickness(pointsTop, pointsBottom, thicknessStepSize);
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

            //try to parse the camber position, if unsucessful (eg not numeric) set to 0.5f
            if (float.TryParse(txtCamberPosition.Text, out float camberPosition) == false || camberPosition < 0.0f || camberPosition > 1.0f)
            {
                txtCamberPosition.Text = "0,5";
                camberPosition = 0.5f;
            }

            //try to parse the camber step size, if unsuccessful (e.g. not numeric) set to 0.001f
            if (float.TryParse(txtCamberStepSize.Text, out float camberStepSize) == false || camberStepSize < 0.00001f || camberStepSize > 1.0f)
            {
                txtCamberStepSize.Text = "0,001";
                camberStepSize = 0.001f;
            }

            List<PointF> camber = GetCamber(pointsTop, pointsBottom, camberPosition, camberStepSize);
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
            errorTop = GetThickness(pointsTop, referenceDatTop, thicknessStepSize);
            totalErrorTop = CalculateAreaUnderCurve(errorTop) * 1000;

            errorBottom.Clear();
            errorBottom = GetThickness(pointsBottom, referenceDatBottom, thicknessStepSize);
            totalErrorBottom = CalculateAreaUnderCurve(errorBottom) * 1000;

            if (totalErrorTop > 0) { txtAirfoilParam.AppendText("error top:\t\t" + totalErrorTop + System.Environment.NewLine); }
            if (totalErrorBottom > 0) { txtAirfoilParam.AppendText("error bottom:\t\t" + totalErrorBottom + System.Environment.NewLine); }


            //----------------------------------------------------------------------------------------------------------------------------------

            txtAirfoilParam.Refresh();
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
            dataGridViewBottom.AllowUserToResizeColumns = false;
            dataGridViewBottom.AllowUserToResizeRows = false;
        }
        private void AddToolTips()
        {
            // Create a new instance of ToolTip
            System.Windows.Forms.ToolTip buttonToolTip = new()
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

            DataGridViewTextBoxColumn columnX = new()
            {
                Name = "xVal",
                HeaderText = "X",
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            DataGridViewTextBoxColumn columnY = new()
            {
                Name = "yVal",
                HeaderText = "Y",
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            dataGridViewTop.Columns.Add(columnX);
            dataGridViewTop.Columns.Add(columnY);
            gridViewAddPoints(dataGridViewTop, defaultControlPointsTop);
        }
        private void AddDefaultPointsBottom()
        {
            dataGridViewBottom.Rows.Clear();
            dataGridViewBottom.Columns.Clear();

            DataGridViewTextBoxColumn columnX = new()
            {
                Name = "xVal",
                HeaderText = "X",
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            DataGridViewTextBoxColumn columnY = new()
            {
                Name = "yVal",
                HeaderText = "Y",
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            dataGridViewBottom.Columns.Add(columnX);
            dataGridViewBottom.Columns.Add(columnY);
            gridViewAddPoints(dataGridViewBottom, defaultControlPointsBottom);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // helper functions


        private async Task SearchTopAsync()
        {
            progressBar1.Visible = true;
            List<PointF> controlPointsTop = GetControlPoints(dataGridViewTop);
            await Task.Run(() => SearchControlPoints(controlPointsTop, dataGridViewTop));
            Console.Beep();
            progressBar1.Visible = false;
        }

        private async Task SearchBottomAsync()
        {
            progressBar1.Visible = true;
            List<PointF> controlPointsBottom = GetControlPoints(dataGridViewBottom);
            await Task.Run(() => SearchControlPoints(controlPointsBottom, dataGridViewBottom));
            Console.Beep();
            progressBar1.Visible = false;
        }

        private void SearchControlPoints(List<PointF> controlPoints, DataGridView gridView)
        {
            if (cancelSearch) return;

            float currentLowestError = (gridView == dataGridViewTop) ? totalErrorTop : totalErrorBottom;
            List<PointF> controlPointsWithLowestError = new(controlPoints); // Store initial state
            int numPoints = 3; // Start with searching two points: current point and one extra point above and below

            bool betterCombinationFound = false;

            while (!betterCombinationFound)
            {
                if (cancelSearch) return;

                float minimumSearchDistance = 0.001f; // Adjust this value as needed.
                float searchDistance = Math.Max(((float)Math.Log(currentLowestError + 1) / 50), minimumSearchDistance);
                float searchStep = searchDistance / (numPoints - 1); // Defines the step size

                // Calculate total combinations and set progress bar maximum.
                int totalCombinations = (int)Math.Pow(numPoints, controlPoints.Count - 2);
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Maximum = totalCombinations;
                    progressBar1.Value = 0; // Reset progress bar value
                });

                SearchCombinations(controlPoints, 1, searchStep, numPoints);

                void SearchCombinations(List<PointF> points, int currentIndex, float step, int numSteps)
                {
                    if (cancelSearch) return;

                    if (currentIndex >= points.Count - 1)
                    {
                        gridViewAddPoints(gridView, points);

                        this.Invoke((MethodInvoker)delegate
                        {
                            // UI update code goes here
                            calculations();
                        });

                        float currentError = (gridView == dataGridViewTop) ? totalErrorTop : totalErrorBottom;
                        if (currentError < currentLowestError)
                        {
                            currentLowestError = currentError;
                            controlPointsWithLowestError.Clear();
                            controlPointsWithLowestError.AddRange(points);
                            betterCombinationFound = true;
                        }

                        // Update progress bar
                        this.Invoke((MethodInvoker)delegate
                        {
                            progressBar1.Value = Math.Min(progressBar1.Value + 1, progressBar1.Maximum);
                        });

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

                this.Invoke((MethodInvoker)delegate // purely cosmetic, ensures that the progress bar is completely filled
                {
                    progressBar1.Maximum = 1;
                });

                Console.Beep();
                numPoints++; // Increase number of points for next iteration
            }

            gridViewAddPoints(gridView, controlPointsWithLowestError);

            this.Invoke((MethodInvoker)delegate
            {
                // UI update code goes here
                calculations();
            });
        }

        private void gridViewAddPoints(DataGridView dataGridView, List<PointF> pointFs)
        {
            if (dataGridView.InvokeRequired)
            {
                dataGridView.BeginInvoke((MethodInvoker)delegate { gridViewAddPoints(dataGridView, pointFs); });
            }
            else
            {
                dataGridView.Rows.Clear();
                for (int i = 0; i < pointFs.Count; i++)
                {
                    dataGridView.Rows.Add(pointFs[i].X, pointFs[i].Y);
                }
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

        private static List<PointF> GetThickness(List<PointF> curve1, List<PointF> curve2, float stepSize)
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
            for (float x = minX; x <= maxX; x += stepSize)  // Adjust the step size as needed.
            {
                float? y1 = InterpolateY(x, curve1);
                float? y2 = InterpolateY(x, curve2);
                if (y1.HasValue && y2.HasValue)
                    distances.Add(new PointF(x, Math.Abs(y1.Value - y2.Value)));
            }

            // Now `distances` contains the vertical distances between the curves at regular intervals.

            return distances;
        }

        private static List<PointF> GetCamber(List<PointF> curve1, List<PointF> curve2, float camberPosition, float stepSize)
        {
            // Ensure the points are sorted by X in ascending order.
            curve1 = curve1.OrderBy(p => p.X).ToList();
            curve2 = curve2.OrderBy(p => p.X).ToList();

            // The X range over which we calculate distances.
            float minX = Math.Max(curve1.First().X, curve2.First().X);
            float maxX = Math.Min(curve1.Last().X, curve2.Last().X);



            // Calculate midpoints at regular intervals within this range.
            List<PointF> midpoints = new();
            for (float x = minX; x <= maxX; x += stepSize)  // Adjust the step size as needed.
            {
                float? y1 = InterpolateY(x, curve1);
                float? y2 = InterpolateY(x, curve2);
                if (y1.HasValue && y2.HasValue)
                {
                    float lowerY = Math.Min(y1.Value, y2.Value);
                    float higherY = Math.Max(y1.Value, y2.Value);
                    float y = lowerY + camberPosition * (higherY - lowerY);
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

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            if (!controlPointsTop.SequenceEqual(defaultControlPointsTop) ||
                !controlPointsBottom.SequenceEqual(defaultControlPointsBottom))
            {
                var result = MessageBox.Show("Closing will discard the modified control points." + System.Environment.NewLine + "Are you sure you want to proceed?", "Unsaved changes", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            cancelSearch = true;
            await Task.Delay(2500);
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
        // textbox events

        private void txtThicknessStepSize_TextChanged(object sender, EventArgs e)
        {
            calculations();
        }

        private void txtCamberStepSize_TextChanged(object sender, EventArgs e)
        {
            calculations();
        }

        private void txtCamberPosition_TextChanged(object sender, EventArgs e)
        {
            calculations();
        }

        private void txtNumOfPointsTop_TextChanged(object sender, EventArgs e)
        {
            calculations();
        }

        private void txtNumOfPointsBottom_TextChanged(object sender, EventArgs e)
        {
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

            for (int i = 0; i <= controlPointsBottom.Count - 1; i++)
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

        private async void btnSearchTop_Click(object sender, EventArgs e)
        {
            btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;

            cancelSearch = false;
            await SearchTopAsync();

            btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
        }

        private async void btnSearchBottom_Click(object sender, EventArgs e)
        {
            btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;

            cancelSearch = false;
            await SearchBottomAsync();

            btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
        }

        private async void btnAutoSearch_Click(object sender, EventArgs e)
        {
            btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;
            cancelSearch = false;

            // Record the start time
            DateTime startTime = DateTime.Now;

            // Record the end time and calculate elapsed time
            DateTime endTime = DateTime.Now;
            TimeSpan elapsedTime = endTime - startTime;

            // Define thresholds
            float errorThresholdTop = 0.075f;
            float errorThresholdBottom = 0.075f;
            float improvementThreshold = 0.05f;

            bool increaseOrderFlag = false;

            while (totalErrorTop > errorThresholdTop || totalErrorBottom > errorThresholdBottom)
            {
                // Check if the operation should be cancelled
                if (cancelSearch) break;

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
                }

                while ((totalErrorTop > errorThresholdTop && errorImprovementTop >= improvementThreshold) ||
                       (totalErrorBottom > errorThresholdBottom && errorImprovementBottom >= improvementThreshold))
                {
                    // Check if the operation should be cancelled
                    if (cancelSearch) break;

                    if (totalErrorTop > errorThresholdTop)
                    {
                        await SearchTopAsync();
                        if (cancelSearch) break;
                        currentErrorTop = totalErrorTop;
                        errorImprovementTop = (previousErrorTop - currentErrorTop) / previousErrorTop;
                        previousErrorTop = currentErrorTop;
                    }

                    if (totalErrorBottom > errorThresholdBottom)
                    {
                        await SearchBottomAsync();
                        if (cancelSearch) break;
                        currentErrorBottom = totalErrorBottom;
                        errorImprovementBottom = (previousErrorBottom - currentErrorBottom) / previousErrorBottom;
                        previousErrorBottom = currentErrorBottom;
                    }

                    endTime = DateTime.Now;
                    elapsedTime = endTime - startTime;

                    lblElapsedTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
                    lblElapsedTime.Refresh();
                }

                endTime = DateTime.Now;
                elapsedTime = endTime - startTime;

                lblElapsedTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
                lblElapsedTime.Refresh();

                increaseOrderFlag = true;
            }

            // If operation wasn't cancelled, play a beep and show a message box with start time, end time, and elapsed time
            if (!cancelSearch)
            {
                // Play a beep
                Console.Beep();
                Console.Beep();
                Console.Beep();

                // Show a message box with start time, end time, and elapsed time
                MessageBox.Show($"Start time: {startTime}\nEnd time: {endTime}\nElapsed time: {elapsedTime}");
            }

            btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
        }

        private void btnStopSearch_Click(object sender, EventArgs e)
        {
            cancelSearch = true;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}