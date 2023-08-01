using BezierAirfoilDesigner.Properties;
using MathNet.Numerics;
using Newtonsoft.Json;
using ScottPlot;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection;
using System.Resources;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BezierAirfoilDesigner
{
    public partial class Form1 : Form
    {

        //--------------------------------------------------------------------------------------------------------------------------------------
        // global variables

        static string currentVersion = "v0.9";

        private static DateTime currentTime;
        private static DateTime startTime;
        private static TimeSpan elapsedTime;

        string loadedAirfoilName = "";

        readonly List<PointD> defaultControlPointsTop = new()
        {
            new PointD(0, 0),
            new PointD(0, 0.15f),
            new PointD(0.5f, 0.15f),
            new PointD(1.0f, 0)
        };
        readonly List<PointD> defaultControlPointsBottom = new()
        {
            new PointD(0, 0),
            new PointD(0, -0.1f),
            new PointD(0.5f, -0.1f),
            new PointD(1.0f, 0)
        };

        List<PointD> referenceDatTop = new();
        List<PointD> referenceDatBottom = new();

        List<PointD> errorTop = new();
        List<PointD> errorBottom = new();

        List<PointD> errorOfEachControlPointTop = new();
        List<PointD> errorOfEachControlPointBottom = new();

        double totalErrorTop;
        double totalErrorBottom;
        double errorThresholdTop;
        double errorThresholdBottom;

        readonly double minZoomRange = 0.01;
        readonly double maxZoomRange = 10.0;

        private static bool showControlTop;
        private static bool showControlBottom;
        private static bool showTop;
        private static bool showBottom;
        private static bool showThickness;
        private static bool showCamber;
        private static bool showRadius;
        private static bool showReferenceTop;
        private static bool showReferenceBottom;

        private static bool cancelSearch;
        private static bool updateCheckTriggeredByButtonClick = false;

        System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip
        {
            AutoPopDelay = 5000,
            InitialDelay = 500,
            ReshowDelay = 500,
            ShowAlways = true
        };

        //--------------------------------------------------------------------------------------------------------------------------------------
        // initilisation

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int margin = 3;
            var activeForm = Form1.ActiveForm;
            if (activeForm != null)
            {
                //activeForm.SuspendLayout();

                //panel1.Left = activeForm.Width - margin - panel1.Width;
                //panel2.Left = activeForm.Width - margin - panel2.Width;

                dataGridViewTop.Left = panel1.Left - margin - dataGridViewTop.Width;
                dataGridViewTop.Height = (activeForm.Height - lblTop.Top - lblTop.Height - margin - margin - lblBottom.Height - margin - (activeForm.Height - lblAirfoilParam.Top)) / 2;
                dataGridViewTop.Width = dataGridViewTop.Columns[0].Width + dataGridViewTop.Columns[1].Width + dataGridViewBottom.RowHeadersWidth + SystemInformation.VerticalScrollBarWidth + 2;
                dataGridViewBottom.Top = dataGridViewTop.Top + dataGridViewTop.Height + 2 * margin + lblBottom.Height;
                dataGridViewBottom.Left = dataGridViewTop.Left;
                dataGridViewBottom.Height = dataGridViewTop.Height;
                dataGridViewBottom.Width = dataGridViewTop.Width;

                txtAirfoilParam.Left = dataGridViewTop.Left;

                panel1.Top = dataGridViewTop.Top;
                panel1.Height = dataGridViewTop.Height;
                panel2.Top = dataGridViewBottom.Top;
                panel2.Height = dataGridViewBottom.Height;

                panel3.Left = dataGridViewTop.Left - panel3.Width - margin - 2;
                panel3.Height = activeForm.Height - dataGridViewTop.Top - (activeForm.Height - (txtAirfoilParam.Top + txtAirfoilParam.Height));

                lblTop.Left = dataGridViewTop.Left;
                lblBottom.Left = dataGridViewTop.Left;
                lblBottom.Top = dataGridViewTop.Top + dataGridViewTop.Height + margin;
                lblAirfoilParam.Left = dataGridViewTop.Left;


                formsPlot1.Width = panel3.Left;
                formsPlot1.Height = txtThicknessStepSize.Top;

                progressBar1.Width = activeForm.Width - 16;


                //activeForm.ResumeLayout();
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await CheckForUpdates();

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

            progressBar1.Visible = false;
            button1.Visible = false;

            cmbLanguage.Items.Add("en");
            cmbLanguage.Items.Add("de");
            cmbLanguage.SelectedIndex = 0;

            calculations();
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // main calculations

        void calculations()
        {
            var axisLimits = formsPlot1.Plot.GetAxisLimits();
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.SetAxisLimits(axisLimits);

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating and plotting points on the top bezier curve

            List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);

            // Try to parse the number of points, if unsuccessful (e.g., not numeric), set to 3
            if (int.TryParse(txtNumOfPointsTop.Text, out int numPointsTop) == false || numPointsTop < 2 || numPointsTop > 100000)
            {
                txtNumOfPointsTop.Text = "225";
                numPointsTop = 225;
            }

            List<PointD> pointsTop = DeCasteljau.BezierCurve(controlPointsTop, numPointsTop);

            Color colorTop;
            if (showTop) { colorTop = Color.Red; } else { colorTop = Color.Transparent; }
            var top = formsPlot1.Plot.AddScatterList(color: colorTop, lineStyle: ScottPlot.LineStyle.Solid);

            for (int i = 0; i < pointsTop.Count; i++)
            {
                top.Add(pointsTop[i].X, pointsTop[i].Y);
            }

            lblOrderTop.Text = "order: " + (controlPointsTop.Count - 1).ToString();

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating and plotting points on the bottom bezier curve

            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            // Try to parse the number of points, if unsuccessful (e.g., not numeric), set to 3
            if (int.TryParse(txtNumOfPointBottom.Text, out int numPointsBottom) == false || numPointsBottom < 2 || numPointsBottom > 100000)
            {
                txtNumOfPointBottom.Text = "225";
                numPointsBottom = 225;
            }

            List<PointD> pointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numPointsBottom);

            Color colorBottom;
            if (showBottom) { colorBottom = Color.Red; } else { colorBottom = Color.Transparent; }
            var bottom = formsPlot1.Plot.AddScatterList(color: colorBottom, lineStyle: ScottPlot.LineStyle.Solid);

            for (int i = 0; i < pointsBottom.Count; i++)
            {
                bottom.Add(pointsBottom[i].X, pointsBottom[i].Y);
            }

            lblOrderBottom.Text = "order: " + (controlPointsBottom.Count - 1).ToString();

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating and plotting the thickness line

            Color thicknessLineColor = new();
            if (showThickness) { thicknessLineColor = Color.Gray; } else { thicknessLineColor = Color.Transparent; }
            var thicknessLine = formsPlot1.Plot.AddScatterList(color: thicknessLineColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);

            // Try to parse the thickness step size, if unsuccessful (e.g. not numeric) set to 0.001f
            if (double.TryParse(txtThicknessStepSize.Text, out double thicknessStepSize) == false || thicknessStepSize < 0.00001f || thicknessStepSize > 1.0f)
            {
                txtThicknessStepSize.Text = "0,001";
                thicknessStepSize = 0.001f;
            }

            List<PointD> thicknesses = GetThickness(pointsTop, pointsBottom, thicknessStepSize);
            PointD maxThickness = new PointD();

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
            if (double.TryParse(txtCamberPosition.Text, out double camberPosition) == false || camberPosition < 0.0f || camberPosition > 1.0f)
            {
                txtCamberPosition.Text = "0,5";
                camberPosition = 0.5f;
            }

            //try to parse the camber step size, if unsuccessful (e.g. not numeric) set to 0.001f
            if (double.TryParse(txtCamberStepSize.Text, out double camberStepSize) == false || camberStepSize < 0.00001f || camberStepSize > 1.0f)
            {
                txtCamberStepSize.Text = "0,001";
                camberStepSize = 0.001f;
            }

            List<PointD> camber = GetCamber(pointsTop, pointsBottom, camberPosition, camberStepSize);
            PointD maxCamber = new PointD();

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

            PointD midpoint = CircleProperties.CalculateMidpoint(pointsBottom[1], pointsTop[0], pointsTop[1]);
            double radius = CircleProperties.CalculateRadius(pointsBottom[1], pointsTop[0], pointsTop[1]);
            Color circleColor;
            if (showRadius) { circleColor = Color.Gray; } else { circleColor = Color.Transparent; }
            formsPlot1.Plot.AddCircle(x: midpoint.X, y: midpoint.Y, radius: radius, color: circleColor, lineWidth: 1, lineStyle: ScottPlot.LineStyle.Dash);

            //----------------------------------------------------------------------------------------------------------------------------------
            // printing airfoil parameters to the text box

            txtAirfoilParam.Text = "";
            txtAirfoilParam.AppendText("nose radius:\t\t" + radius + System.Environment.NewLine);
            txtAirfoilParam.AppendText("maximum camber:\t" + maxCamber.Y.ToString() + System.Environment.NewLine + "\tat:\t\t" + maxCamber.X.ToString() + System.Environment.NewLine);
            txtAirfoilParam.AppendText("maximum thickness:\t" + maxThickness.Y.ToString() + System.Environment.NewLine + "\tat:\t\t" + maxThickness.X.ToString() + System.Environment.NewLine);

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
            errorTop = GetThickness(pointsTop, referenceDatTop, 0.001f);
            totalErrorTop = CalculateAreaUnderCurve(errorTop) * 1000;

            //totalErrorTop = 0;

            //for (int i = 0; i < errorTop.Count; i++)
            //{
            //    double newY = errorTop[i].Y * ((2 * errorTop.Count - i) / errorTop.Count);
            //    errorTop[i] = new PointD(errorTop[i].X, newY);
            //}


            errorOfEachControlPointTop.Clear();

            for (int i = 0; i < controlPointsTop.Count; i++)
            {
                double targetX = controlPointsTop[i].X;  // The x-value you're interested in
                double delta = 1 / (double)(controlPointsTop.Count - 1);  // The range around the x-value

                double ySum = errorTop.Where(point => Math.Abs(point.X - targetX) <= delta).Sum(point => point.Y);

                errorOfEachControlPointTop.Add(new PointD(targetX, ySum));
            }




            errorBottom.Clear();
            errorBottom = GetThickness(pointsBottom, referenceDatBottom, 0.001f);
            totalErrorBottom = CalculateAreaUnderCurve(errorBottom) * 1000;

            //totalErrorBottom = 0;

            //for (int i = 0; i < errorBottom.Count; i++)
            //{
            //    double newY = errorBottom[i].Y * ((2 * errorBottom.Count - i) / errorBottom.Count);
            //    errorBottom[i] = new PointD(errorBottom[i].X, newY);
            //}


            errorOfEachControlPointBottom.Clear();

            for (int i = 0; i < controlPointsBottom.Count; i++)
            {
                double targetX = controlPointsBottom[i].X;  // The x-value you're interested in
                double delta = 1 / (double)(controlPointsBottom.Count - 1);  // The range around the x-value

                double ySum = errorBottom.Where(point => Math.Abs(point.X - targetX) <= delta).Sum(point => point.Y);

                errorOfEachControlPointBottom.Add(new PointD(targetX, ySum));
            }

            if (totalErrorTop > 0) { txtAirfoilParam.AppendText(System.Environment.NewLine + "error top:\t\t" + totalErrorTop + System.Environment.NewLine); }
            if (totalErrorBottom > 0) { txtAirfoilParam.AppendText("error bottom:\t\t" + totalErrorBottom + System.Environment.NewLine); }

            // Append each control point error to txtAirfoilParam
            //for (int i = 0; i < errorOfEachControlPointTop.Count; i++)
            //{
            //    txtAirfoilParam.AppendText($"Error at control point {i} (x={errorOfEachControlPointTop[i].X}): {errorOfEachControlPointTop[i].Y}" + System.Environment.NewLine);
            //}

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

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddToolTips();
        }

        private void AddToolTips()
        {
            // Define the controls
            string[] controls = {
                "btnDefault", "btnLoadDat", "btnLoadBezDat", "btnLoadBez", "btnSearchTop",
                "btnSearchBottom", "btnAutoSearch", "btnStopSearch", "btnSaveDat",
                "btnSaveBezDat", "btnSaveBez", "chkShowControlTop", "chkShowControlBottom",
                "chkShowTop", "chkShowBottom", "chkShowThickness", "txtThicknessStepSize",
                "chkShowCamber", "txtCamberStepSize", "txtCamberPosition", "chkShowRadius",
                "chkShowReferenceTop", "chkShowReferenceBottom", "btnIncreaseOrderTop",
                "btnDecreaseOrderTop", "txtNumOfPointsTop", "btnIncreaseOrderBottom",
                "btnDecreaseOrderBottom", "txtNumOfPointBottom", "btnAxisAuto"
            };

            // Set new tooltips
            foreach (string control in controls)
            {
                var toolTipText = cmbLanguage.SelectedIndex == 1 ?
                    ToolTips_de.ResourceManager.GetString(control) as string :
                    ToolTips_en.ResourceManager.GetString(control) as string;

                System.Windows.Forms.Control foundControl = FindControlRecursive(this, control);
                if (foundControl != null)
                {
                    toolTip.SetToolTip(foundControl, toolTipText);
                }
            }
        }

        static public System.Windows.Forms.Control FindControlRecursive(System.Windows.Forms.Control container, string name)
        {
            if (container.Name == name) return container;

            foreach (System.Windows.Forms.Control ctrl in container.Controls)
            {
                System.Windows.Forms.Control foundCtrl = FindControlRecursive(ctrl, name);
                if (foundCtrl != null) return foundCtrl;
            }

            return null;
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

            double improvementThreshold = 0.01f;

            double previousError = totalErrorTop;
            double currentError;
            double errorImprovement = double.PositiveInfinity;

            while (errorImprovement >= improvementThreshold)
            {
                // Check if the operation should be cancelled
                if (cancelSearch) break;

                List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);
                await Task.Run(() => SearchControlPoints(controlPointsTop, dataGridViewTop));
                Console.Beep();
                currentError = totalErrorTop;
                errorImprovement = (previousError - currentError) / previousError;
                previousError = currentError;
            }

            progressBar1.Visible = false;
        }

        private async Task SearchBottomAsync()
        {
            progressBar1.Visible = true;

            double improvementThreshold = 0.01f;

            double previousError = totalErrorBottom;
            double currentError;
            double errorImprovement = double.PositiveInfinity;

            while (errorImprovement >= improvementThreshold)
            {
                // Check if the operation should be cancelled
                if (cancelSearch) break;

                List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);
                await Task.Run(() => SearchControlPoints(controlPointsBottom, dataGridViewBottom));
                Console.Beep();
                currentError = totalErrorBottom;
                errorImprovement = (previousError - currentError) / previousError;
                previousError = currentError;
            }

            progressBar1.Visible = false;
        }

        private void SearchControlPoints(List<PointD> controlPoints, DataGridView gridView)
        {
            if (cancelSearch) return;

            bool topOrBottom = gridView == dataGridViewTop;  // Set to true for top, false for bottom

            double currentLowestError = topOrBottom ? totalErrorTop : totalErrorBottom;

            List<PointD> controlPointsWithLowestError = new(controlPoints); // Store initial state
            int numPoints = 3; // Start with searching two points: current point and one extra point above and below

            bool betterCombinationFound = false;

            while (!betterCombinationFound)
            {
                double minimumSearchDistance = 0.001f; // Adjust this value as needed.
                double searchDistance = Math.Max(((double)Math.Log(currentLowestError + 1) / 50), minimumSearchDistance);
                double searchStep = searchDistance / (numPoints - 1); // Defines the step size

                // Calculate total combinations and set progress bar maximum.
                int totalCombinations = (int)Math.Pow(numPoints, controlPoints.Count - 2);
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Maximum = totalCombinations;
                    progressBar1.Value = 0; // Reset progress bar value
                });

                SearchCombinations(controlPoints, 1, numPoints, topOrBottom);

                void SearchCombinations(List<PointD> points, int currentIndex, int numSteps, bool topOrBottom)
                {
                    if (cancelSearch)
                    {
                        betterCombinationFound = true;
                        return;
                    };

                    if(topOrBottom)
                    {
                        if (currentLowestError < errorThresholdTop) return;
                    }
                    else
                    {
                        if (currentLowestError < errorThresholdBottom) return;
                    }

                    if (currentIndex >= points.Count - 1)
                    {
                        gridViewAddPoints(gridView, points);

                        this.Invoke((MethodInvoker)delegate
                        {
                            // UI update code goes here
                            calculations();
                            txtAirfoilParam.AppendText(System.Environment.NewLine + "current lowest error:\t" + currentLowestError + System.Environment.NewLine);
                            txtAirfoilParam.Refresh();

                            progressBar1.Value = Math.Min(progressBar1.Value + 1, progressBar1.Maximum);

                            currentTime = DateTime.Now;
                            elapsedTime = currentTime - startTime;
                            System.Windows.Forms.Control control = panel3.Controls.Find("lblElapsedTime", true)[0];
                            control.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
                            control.Refresh();
                        });

                        double currentError = (gridView == dataGridViewTop) ? totalErrorTop : totalErrorBottom;

                        if (currentError < currentLowestError)
                        {
                            currentLowestError = currentError;
                            controlPointsWithLowestError.Clear();
                            controlPointsWithLowestError.AddRange(points);
                            betterCombinationFound = true;
                        }

                        return;
                    }

                    PointD originalPoint = points[currentIndex];
                    double searchMin = originalPoint.Y - searchDistance / 2; // Start from below the current Y-coordinate

                    // Iterates over numPoints specific points around the current Y-coordinate
                    for (int i = 0; i < numSteps; i++)
                    {
                        double y = searchMin + i * searchStep;
                        points[currentIndex] = new PointD(originalPoint.X, y);
                        SearchCombinations(points, currentIndex + 1, numSteps, topOrBottom);
                    }

                    points[currentIndex] = originalPoint; // Restore original point before returning.
                }

                this.Invoke((MethodInvoker)delegate // purely cosmetic, ensures that the progress bar is completely filled
                {
                    progressBar1.Maximum = 1;
                });

                Console.Beep();
                numPoints += 2; // Increase number of points for next iteration
            }

            gridViewAddPoints(gridView, controlPointsWithLowestError);

            this.Invoke((MethodInvoker)delegate
            {
                // UI update code goes here
                calculations();
            });
        }

        private void gridViewAddPoints(DataGridView dataGridView, List<PointD> PointDs)
        {
            if (dataGridView.InvokeRequired)
            {
                dataGridView.BeginInvoke((MethodInvoker)delegate { gridViewAddPoints(dataGridView, PointDs); });
            }
            else
            {
                dataGridView.Rows.Clear();
                for (int i = 0; i < PointDs.Count; i++)
                {
                    dataGridView.Rows.Add(PointDs[i].X, PointDs[i].Y);
                }
            }
        }

        private static double GetDistanceBetweenPoints(PointD pointA, PointD pointB)
        {
            double distanceX = pointA.X - pointB.X;
            double distanceY = pointA.Y - pointB.Y;
            double distance = double.Parse((Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2))).ToString());
            return distance;
        }

        private static List<PointD> GetControlPoints(DataGridView gridView)
        {
            List<PointD> controlPointsBottom = new();

            for (int i = 0; i < gridView.Rows.Count - 1; i++)
            {
                // Retrieve the values from the DataGridView
                _ = double.TryParse(s: gridView.Rows[i].Cells[0].Value.ToString(), out double x);
                _ = double.TryParse(s: gridView.Rows[i].Cells[1].Value.ToString(), out double y);
                // Create a PointD object
                PointD point = new(x, y);
                controlPointsBottom.Add(point);
            }

            return controlPointsBottom;
        }

        private static List<PointD> GetThickness(List<PointD> curve1, List<PointD> curve2, double stepSize)
        {
            List<PointD> distances = new();

            if (!curve1.Any() || !curve2.Any()) { return distances; }

            // Ensure the points are sorted by X in ascending order.
            curve1 = curve1.OrderBy(p => p.X).ToList();
            curve2 = curve2.OrderBy(p => p.X).ToList();

            // The X range over which we calculate distances.
            double minX = Math.Max(curve1.First().X, curve2.First().X);
            double maxX = Math.Min(curve1.Last().X, curve2.Last().X);

            // Calculate vertical distances at regular intervals within this range.
            for (double x = minX; x <= maxX; x += stepSize)  // Adjust the step size as needed.
            {
                double? y1 = InterpolateY(x, curve1);
                double? y2 = InterpolateY(x, curve2);
                if (y1.HasValue && y2.HasValue)
                    distances.Add(new PointD(x, Math.Abs(y1.Value - y2.Value)));
            }

            // Now `distances` contains the vertical distances between the curves at regular intervals.

            return distances;
        }

        private static List<PointD> GetCamber(List<PointD> curve1, List<PointD> curve2, double camberPosition, double stepSize)
        {
            // Ensure the points are sorted by X in ascending order.
            curve1 = curve1.OrderBy(p => p.X).ToList();
            curve2 = curve2.OrderBy(p => p.X).ToList();

            // The X range over which we calculate distances.
            double minX = Math.Max(curve1.First().X, curve2.First().X);
            double maxX = Math.Min(curve1.Last().X, curve2.Last().X);



            // Calculate midpoints at regular intervals within this range.
            List<PointD> midpoints = new();
            for (double x = minX; x <= maxX; x += stepSize)  // Adjust the step size as needed.
            {
                double? y1 = InterpolateY(x, curve1);
                double? y2 = InterpolateY(x, curve2);
                if (y1.HasValue && y2.HasValue)
                {
                    double lowerY = Math.Min(y1.Value, y2.Value);
                    double higherY = Math.Max(y1.Value, y2.Value);
                    double y = lowerY + camberPosition * (higherY - lowerY);
                    midpoints.Add(new PointD(x, y));
                }
            }


            // Now `midpoints` contains the midpoints between the curves at regular intervals,
            // stored as PointD where X is the x-value and Y is the midpoint Y value.

            return midpoints;
        }

        static double? InterpolateY(double x, List<PointD> curve)
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

        public static double CalculateAreaBetweenCurves(List<PointD> curve1Points, List<PointD> curve2Points)
        {
            double areaCurve1 = CalculateAreaUnderCurve(curve1Points);
            double areaCurve2 = CalculateAreaUnderCurve(curve2Points);

            return Math.Abs(areaCurve1 - areaCurve2);
        }

        public static double CalculateAreaUnderCurve(List<PointD> curvePoints)
        {
            double area = 0.0f;

            for (int i = 0; i < curvePoints.Count - 1; i++)
            {
                double h = curvePoints[i + 1].X - curvePoints[i].X;
                double avgY = (curvePoints[i].Y + curvePoints[i + 1].Y) / 2.0f;
                area += h * avgY;
            }

            return area;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // update check

        public static async Task CheckForUpdates()
        {
            string userAgent = $"Mozilla/5.0 (compatible; BezierAirfoilDesigner/{currentVersion})";

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
                    var (latestVersion, latestReleaseUrl) = await GetLatestVersion(client);
                    int result = CompareVersions(currentVersion, latestVersion);
                    if (result < 0)
                    {
                        string message = $"An update is available! Current version: {currentVersion}, latest version: {latestVersion}\n\n" +
                                         $"Click OK to go to the latest release.";
                        DialogResult dialogResult = MessageBox.Show(message, "Update Available", MessageBoxButtons.OKCancel);
                        if (dialogResult == DialogResult.OK)
                        {
                            OpenUrl(latestReleaseUrl);
                        }
                    }
                    else if (result == 0 && updateCheckTriggeredByButtonClick)
                    {
                        MessageBox.Show($"You are running the latest version: {currentVersion}", "Up to Date");
                    }
                    else if (result > 0)
                    {
                        MessageBox.Show($"Your version {currentVersion} is newer than the latest release {latestVersion}", "Up to Date");
                    }
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Unable to connect to the internet. Please check your internet connection and try again.", "Error");
            }
            catch (WebException)
            {
                MessageBox.Show("Unable to connect to the internet. Please check your internet connection and try again.", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred. Please try again later.", "Error");
            }
        }

        private static async Task<(string version, string url)> GetLatestVersion(HttpClient client)
        {
            var response = await client.GetAsync("https://api.github.com/repos/Marc-Frank/BezierAirfoilDesigner/releases/latest");
            response.EnsureSuccessStatusCode();
            dynamic latestRelease = Newtonsoft.Json.JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            return (latestRelease.tag_name.ToString(), latestRelease.html_url.ToString());
        }

        private static int CompareVersions(string currentVersion, string latestVersion)
        {
            Version current = new Version(currentVersion.TrimStart('v'));
            Version latest = new Version(latestVersion.TrimStart('v'));
            return current.CompareTo(latest);
        }

        public static void OpenUrl(string url)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // form events

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);

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
            List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            (double mouseCoordX, double mouseCoordY) = formsPlot1.GetMouseCoordinates();
            PointD mouse = new(double.Parse(mouseCoordX.ToString()), double.Parse(mouseCoordY.ToString()));

            double lowestDistanceTop = double.PositiveInfinity;
            int indexLowestDistanceTop = 0;
            double lowestDistanceBottom = double.PositiveInfinity;
            int indexLowestDistanceBottom = 0;

            double lowestDistance = double.PositiveInfinity;
            int indexLowestDistance = 0;
            bool topOrBottom;

            for (int i = 0; i < controlPointsTop.Count; i++)
            {
                double currentDistance = GetDistanceBetweenPoints(mouse, controlPointsTop[i]);
                if (currentDistance < lowestDistanceTop)
                {
                    lowestDistanceTop = currentDistance;
                    indexLowestDistanceTop = i;
                }
            }
            for (int i = 0; i < controlPointsBottom.Count; i++)
            {
                double currentDistance = GetDistanceBetweenPoints(controlPointsBottom[i], mouse);
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

            if (indexLowestDistance >= 0) //protect against dragging the LE point
            {
                if (topOrBottom /*&& indexLowestDistance < controlPointsTop.Count - 1*/) //chose top or bottom and protect against dragging the TE point
                {
                    controlPointsTop[indexLowestDistance] = mouse;
                    gridViewAddPoints(dataGridViewTop, controlPointsTop);
                }
                else if (!topOrBottom /*&& indexLowestDistance < controlPointsBottom.Count - 1*/) //chose top or bottom and protect against dragging the TE point
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

        private void btnAxisAuto_Click(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }

        private void btnIncreaseOrderTop_Click(object sender, EventArgs e)
        {
            List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);
            controlPointsTop = DeCasteljau.IncreaseOrder(controlPointsTop);
            gridViewAddPoints(dataGridViewTop, controlPointsTop);
            calculations();
        }

        private void btnIncreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);
            controlPointsBottom = DeCasteljau.IncreaseOrder(controlPointsBottom);
            gridViewAddPoints(dataGridViewBottom, controlPointsBottom);
            calculations();
        }

        private void btnDecreaseOrderTop_Click(object sender, EventArgs e)
        {
            List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);
            controlPointsTop = DeCasteljau.DecreaseOrder(controlPointsTop);
            gridViewAddPoints(dataGridViewTop, controlPointsTop);
            calculations();
        }

        private void btnDecreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);
            controlPointsBottom = DeCasteljau.DecreaseOrder(controlPointsBottom);
            gridViewAddPoints(dataGridViewBottom, controlPointsBottom);
            calculations();
        }

        private void btnSaveDat_Click(object sender, EventArgs e)
        {
            // Get the control points from the data grid views
            List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            // Ensure that the number of points for top and bottom is at least 3
            if (int.Parse(txtNumOfPointsTop.Text) < 3) { txtNumOfPointsTop.Text = "3"; }
            if (int.Parse(txtNumOfPointBottom.Text) < 3) { txtNumOfPointBottom.Text = "3"; }

            // Parse the number of points for top and bottom from the text boxes
            int numPointsTop = int.Parse(txtNumOfPointsTop.Text);
            int numPointsBottom = int.Parse(txtNumOfPointBottom.Text);

            // Calculate the bezier curves for top and bottom using the De Casteljau's algorithm
            List<PointD> pointsTop = DeCasteljau.BezierCurve(controlPointsTop, numPointsTop);
            List<PointD> pointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numPointsBottom);

            // Remove "Bezier " prefix from the loaded airfoil name if it exists
            string airfoilName = loadedAirfoilName.StartsWith("Bezier ") ? loadedAirfoilName.Substring(7) : loadedAirfoilName;

            // Construct the .dat file content, using the airfoil name as the first line
            string fileContents = "Bezier " + airfoilName + System.Environment.NewLine;

            for (int i = pointsTop.Count - 1; i >= 0; i--)
            {
                fileContents += ($"{pointsTop[i].X:N8} {pointsTop[i].Y:N8}" + System.Environment.NewLine);
            }

            for (int i = 1; i <= pointsBottom.Count - 1; i++)
            {
                fileContents += ($"{pointsBottom[i].X:N8} {pointsBottom[i].Y:N8}" + System.Environment.NewLine);
            }

            // Replace comma with dot in the decimal point (to conform to the .dat file format)
            fileContents = fileContents.Replace(',', '.');

            // Create a new SaveFileDialog instance and set its properties
            using SaveFileDialog sfd = new();
            sfd.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            sfd.FileName = "Bezier " + airfoilName;  // Set the initial filename in the dialog

            // Show the dialog and continue only if a file was selected (OK result)
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Save the .dat file content to the selected file
                SaveTextToFile(fileContents, sfd.FileName);
            }
        }

        private void btnSaveBezDat_Click(object sender, EventArgs e)
        {
            // Get the control points from the data grid views
            List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            // Remove "Bezier " prefix from the loaded airfoil name if it exists
            string airfoilName = loadedAirfoilName.StartsWith("Bezier ") ? loadedAirfoilName.Substring(7) : loadedAirfoilName;

            // Start constructing the .bez.dat file content, using the airfoil name as the first line
            string fileContents = airfoilName + System.Environment.NewLine;

            // Append the control points for the top curve
            for (int i = controlPointsTop.Count - 1; i >= 0; i--)
            {
                fileContents += ($"{controlPointsTop[i].X:N8} {controlPointsTop[i].Y:N8}" + System.Environment.NewLine);
            }

            // Append the control points for the bottom curve
            for (int i = 1; i <= controlPointsBottom.Count - 1; i++)
            {
                fileContents += ($"{controlPointsBottom[i].X:N8} {controlPointsBottom[i].Y:N8}" + System.Environment.NewLine);
            }

            // Replace comma with dot in the decimal point (to conform to the .dat file format)
            fileContents = fileContents.Replace(',', '.');

            // Create a new SaveFileDialog instance and set its properties
            using SaveFileDialog sfd = new();
            sfd.Filter = "Bezier dat files (*.bez.dat)|*.bez.dat|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            // Set the initial filename in the dialog
            sfd.FileName = airfoilName;

            // Show the dialog and continue only if a file was selected (OK result)
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Save the .bez.dat file content to the selected file
                SaveTextToFile(fileContents, sfd.FileName);
            }
        }

        private void btnSaveBez_Click(object sender, EventArgs e)
        {
            // Get the control points from the data grid views
            List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);
            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            // Remove "Bezier " prefix from the loaded airfoil name if it exists
            string airfoilName = loadedAirfoilName.StartsWith("Bezier ") ? loadedAirfoilName.Substring(7) : loadedAirfoilName;

            // Start constructing the .bez file content, using the airfoil name as the first line
            string fileContents = airfoilName + System.Environment.NewLine;

            // Append the control points for the top curve
            fileContents += "Top Start" + System.Environment.NewLine;
            for (int i = 0; i <= controlPointsTop.Count - 1; i++)
            {
                fileContents += ($"{controlPointsTop[i].X:N8} {controlPointsTop[i].Y:N8}" + System.Environment.NewLine);
            }
            fileContents += "Top End" + System.Environment.NewLine;

            // Append the control points for the bottom curve
            fileContents += "Bottom Start" + System.Environment.NewLine;
            for (int i = 0; i <= controlPointsBottom.Count - 1; i++)
            {
                fileContents += ($"{controlPointsBottom[i].X:N8} {controlPointsBottom[i].Y:N8}" + System.Environment.NewLine);
            }
            fileContents += "Bottom End" + System.Environment.NewLine;

            // Replace comma with dot in the decimal point (to conform to the .bez file format)
            fileContents = fileContents.Replace(',', '.');

            // Create a new SaveFileDialog instance and set its properties
            using SaveFileDialog sfd = new();
            sfd.Filter = "Bezier files (*.bez)|*.bez|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            sfd.FileName = airfoilName;  // Set the initial filename in the dialog

            // Show the dialog and continue only if a file was selected (OK result)
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Save the .bez file content to the selected file
                SaveTextToFile(fileContents, sfd.FileName);
            }
        }

        private void btnLoadDat_Click(object sender, EventArgs e)
        {
            referenceDatTop.Clear();
            referenceDatBottom.Clear();

            List<PointD> pointsReferenceAirfoil = new();

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

                // header line is written to the global variable, for use in saving
                var line = reader.ReadLine();
                if (line != null)
                {
                    loadedAirfoilName = line;
                }
                else
                {
                    // Handle the situation when the file is empty or we've reached the end of the file
                    // For example, you might throw an exception or assign a default value to loadedAirfoilName
                    loadedAirfoilName = "airfoil name";
                }

                while ((line = reader.ReadLine()) != null)
                {
                    // split the line on whitespace
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 2)
                    {
                        // parse the parts as doubles
                        if (double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double x)
                        && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
                        {
                            // add the point to the list
                            pointsReferenceAirfoil.Add(new PointD(x, y));
                        }
                    }
                }
            }
            else { return; } // The user did not select a file and clicked Cancel, so exit the method

            int index = 0;
            double minumum = double.PositiveInfinity;

            for (int i = 1; i < pointsReferenceAirfoil.Count; i++)
            {
                if (Math.Abs(pointsReferenceAirfoil[i].X) + Math.Abs(pointsReferenceAirfoil[i].Y) < minumum)
                {
                    minumum = Math.Abs(pointsReferenceAirfoil[i].X) + Math.Abs(pointsReferenceAirfoil[i].Y);
                    index = i;
                }
            }

            referenceDatTop = pointsReferenceAirfoil.GetRange(0, index + 1);  // From start to minimum LE point has to be duplicated in split
            referenceDatTop.Reverse();

            referenceDatBottom = pointsReferenceAirfoil.GetRange(index, pointsReferenceAirfoil.Count - index);  // From minimum to end

            chkShowReferenceTop.Enabled = true;
            chkShowReferenceBottom.Enabled = true;
            chkShowReferenceTop.Checked = true;
            chkShowReferenceBottom.Checked = true;

            btnSearchTop.Enabled = true;
            btnSearchBottom.Enabled = true;
            btnAutoSearch.Enabled = true;

            this.Text = "BezierAirfoilDesigner  -  Loaded reference airfoil: " + loadedAirfoilName;

            calculations();

            //if totalErrorTop or totalErrorBottom is 0 or NaN or not numeric, show an error message
            if (double.IsNaN(totalErrorTop) || double.IsNaN(totalErrorBottom) || double.IsInfinity(totalErrorTop) || double.IsInfinity(totalErrorBottom) || totalErrorTop == 0 || totalErrorBottom == 0)
            {
                MessageBox.Show("The reference airfoil has been loaded successfully, but there was an error processing it. Please check the airfoil and try again.", "Error processing Airfoil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnLoadBezDat_Click(object sender, EventArgs e)
        {
            List<PointD> controlPoints = new();

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
                string? line = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    // split the line on whitespace
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 2)
                    {
                        // parse the parts as doubles
                        if (double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double x)
                        && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
                        {
                            // add the point to the list
                            controlPoints.Add(new PointD(x, y));
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

            List<PointD> controlPointsTop = controlPoints.GetRange(0, index + 1);  // From start to minimum (inclusive)
            controlPointsTop.Reverse(); //control Points are stored just like in a .dat from TE over the top to LE under the bottom to TE
                                        //LE point has to be duplicated in split

            List<PointD> controlPointsBottom = controlPoints.GetRange(index, controlPoints.Count - index);  // From minimum (exclusive) to end

            gridViewAddPoints(dataGridViewTop, controlPointsTop);
            gridViewAddPoints(dataGridViewBottom, controlPointsBottom);

            calculations();
        }

        private void btnLoadBez_Click(object sender, EventArgs e)
        {
            // Initialize lists to store control points for top and bottom curves
            List<PointD> controlPointsTop = new();
            List<PointD> controlPointsBottom = new();

            // Create a new OpenFileDialog instance and set the file filter to only show .bez.dat files
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Bezier files (*.bez)|*.bez"
            };

            // Show the file dialog and continue only if a file was selected (OK result)
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Retrieve the full path of the selected file
                string path = openFileDialog.FileName;

                // Create a new StreamReader instance to read from the selected file
                using StreamReader reader = new(path);

                // Read the first line from the file (and ignore it, as it's a header)
                string line = reader.ReadLine();

                // Variable to keep track of which list of control points we're currently adding to
                List<PointD> currentControlPoints = null;

                // Read each line from the file until no more lines are available
                while ((line = reader.ReadLine()) != null)
                {
                    // Split the line into parts, using spaces or tabs as separators
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    // If the line is a "Start" line...
                    if (parts.Length >= 2 && parts[0] == "Top" && parts[1] == "Start")
                    {
                        // ...we're starting to read the top curve control points
                        currentControlPoints = controlPointsTop;
                    }
                    else if (parts.Length >= 2 && parts[0] == "Bottom" && parts[1] == "Start")
                    {
                        // ...we're starting to read the bottom curve control points
                        currentControlPoints = controlPointsBottom;
                    }
                    // If the line is an "End" line...
                    else if ((parts.Length >= 2 && parts[0] == "Top" && parts[1] == "End") ||
                             (parts.Length >= 2 && parts[0] == "Bottom" && parts[1] == "End"))
                    {
                        // ...we've finished reading the current curve control points
                        currentControlPoints = null;
                    }
                    // If the line is a control point line...
                    else if (parts.Length == 2 && currentControlPoints != null)
                    {
                        // ...try to parse the parts as doubleing-point numbers
                        if (double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double x)
                        && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
                        {
                            // ...and if successful, add the point to the current control points list
                            currentControlPoints.Add(new PointD(x, y));
                        }
                    }
                }
            }
            else
            {
                // If no file was selected, exit the method
                return;
            }

            //// Sort the control points for each curve by X, then by Y (descending for bottom curve)
            //controlPointsTop = controlPointsTop.OrderBy(point => point.X).ThenBy(point => point.Y).ToList();
            //controlPointsBottom = controlPointsBottom.OrderBy(point => point.X).ThenByDescending(point => point.Y).ToList();

            // After loading, if the last point's X value is less than the first point's X value, reverse the list
            if (controlPointsTop.Last().X < controlPointsTop.First().X)
            {
                controlPointsTop.Reverse();
            }
            if (controlPointsBottom.Last().X < controlPointsBottom.First().X)
            {
                controlPointsBottom.Reverse();
            }

            // Add the control points to their respective DataGridViews
            gridViewAddPoints(dataGridViewTop, controlPointsTop);
            gridViewAddPoints(dataGridViewBottom, controlPointsBottom);

            // Perform any necessary calculations after loading the control points
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

                this.Text = "BezierAirfoilDesigner";
                loadedAirfoilName = "";
            }

            calculations();
        }

        private async void btnSearchTop_Click(object sender, EventArgs e)
        {
            btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;

            cancelSearch = false;

            startTime = DateTime.Now;

            await SearchTopAsync();

            btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
        }

        private async void btnSearchBottom_Click(object sender, EventArgs e)
        {
            btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;

            cancelSearch = false;

            startTime = DateTime.Now;

            await SearchBottomAsync();

            btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
        }

        private async void btnAutoSearch_Click(object sender, EventArgs e)
        {
            btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;
            cancelSearch = false;

            // Record the start time
            startTime = DateTime.Now;

            // Define thresholds
            errorThresholdTop = 0.075f;
            errorThresholdBottom = 0.075f;
            //double improvementThreshold = 0.05f;

            double previousError = totalErrorTop + totalErrorBottom;
            double currentError;
            double errorImprovement = 1.0f;

            while (/*errorImprovement >= improvementThreshold*/ totalErrorTop > errorThresholdTop || totalErrorBottom > errorThresholdBottom)
            {
                // Check if the operation should be cancelled
                if (cancelSearch) break;

                await SearchTopAsync();
                await SearchBottomAsync();

                btnIncreaseOrderTop.PerformClick();
                btnIncreaseOrderBottom.PerformClick();

                currentError = totalErrorTop + totalErrorBottom;
                errorImprovement = (previousError - currentError) / previousError;
                //previousError = currentError;
            }

            // If operation wasn't cancelled, play a beep and show a message box with start time, end time, and elapsed time
            if (!cancelSearch)
            {
                // Play a beep
                Console.Beep();
                Console.Beep();
                Console.Beep();

                // Show a message box with start time, end time, and elapsed time
                MessageBox.Show($"Start time: {startTime}\nEnd time: {currentTime}\nElapsed time: {elapsedTime}");
            }

            btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
        }

        private void btnStopSearch_Click(object sender, EventArgs e)
        {
            cancelSearch = true;
        }

        private async void btnCheckForUpdates_Click(object sender, EventArgs e)
        {
            updateCheckTriggeredByButtonClick = true;
            await CheckForUpdates();
            updateCheckTriggeredByButtonClick = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // List of points that form the airfoil
            List<PointD> points = new();
            points.AddRange(referenceDatTop);
            //points.AddRange(referenceDatBottom);

            // Initialize list to store tangent vectors at each point
            List<PointD> tangents = new List<PointD>();

            // Step 2: Compute tangents at each point
            for (int i = 0; i < points.Count; i++)
            {
                if (i == 0) // For the first point, the tangent is towards the next point
                {
                    // Compute direction from current point to next point
                    double dx = points[i + 1].X - points[i].X;
                    double dy = points[i + 1].Y - points[i].Y;

                    // Compute length of direction vector
                    double length = (double)Math.Sqrt(dx * dx + dy * dy);

                    // Normalize direction vector
                    PointD direction = new PointD(dx / length, dy / length);

                    tangents.Add(direction);
                }
                else if (i == points.Count - 1) // For the last point, the tangent is from the previous point
                {
                    // Compute direction from previous point to current point
                    double dx = points[i].X - points[i - 1].X;
                    double dy = points[i].Y - points[i - 1].Y;

                    // Compute length of direction vector
                    double length = (double)Math.Sqrt(dx * dx + dy * dy);

                    // Normalize direction vector
                    PointD direction = new PointD(dx / length, dy / length);

                    tangents.Add(direction);
                }
                else // For any other point, the tangent is the average of the direction from the previous point and towards the next point
                {
                    // Compute direction from previous point to current point
                    double dx1 = points[i].X - points[i - 1].X;
                    double dy1 = points[i].Y - points[i - 1].Y;

                    // Compute length of direction vector
                    double length1 = (double)Math.Sqrt(dx1 * dx1 + dy1 * dy1);

                    // Normalize direction vector
                    PointD direction1 = new PointD(dx1 / length1, dy1 / length1);

                    // Compute direction from current point to next point
                    double dx2 = points[i + 1].X - points[i].X;
                    double dy2 = points[i + 1].Y - points[i].Y;

                    // Compute length of direction vector
                    double length2 = (double)Math.Sqrt(dx2 * dx2 + dy2 * dy2);

                    // Normalize direction vector
                    PointD direction2 = new PointD(dx2 / length2, dy2 / length2);

                    // Compute average of both directions to get tangent vector
                    PointD tangent = new PointD((direction1.X + direction2.X) / 2, (direction1.Y + direction2.Y) / 2);

                    tangents.Add(tangent);
                }
            }

            // Store each set of control points for a curve segment between two points on the airfoil
            List<List<PointD>> controlPointsSegments = new List<List<PointD>>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                PointD P0 = points[i];
                PointD P3 = points[i + 1];
                PointD T0 = tangents[i];
                PointD T3 = tangents[i + 1];

                double distance = (double)Math.Sqrt(Math.Pow(P3.X - P0.X, 2) + Math.Pow(P3.Y - P0.Y, 2));

                PointD P1;

                if (i == 0) // For the first point, make the tangent vertical
                {
                    // Set the x-coordinate of the first control point to be the same as the x-coordinate of the first point of the airfoil
                    double x1 = P0.X;

                    // Set the y-coordinate of the first control point such that the tangent is vertical (x-component of the tangent is zero)
                    double y1 = P0.Y + distance / 3;

                    P1 = new PointD(x1, y1);
                }
                else // For other points, calculate the control point as before
                {
                    P1 = new PointD(P0.X + T0.X * distance / 3, P0.Y + T0.Y * distance / 3);
                }

                PointD P2 = new PointD(P3.X - T3.X * distance / 3, P3.Y - T3.Y * distance / 3);

                // Store the control points for the current curve segment in a list
                List<PointD> controlPoints = new List<PointD>();
                controlPoints.Add(P0);
                controlPoints.Add(P1);
                controlPoints.Add(P2);
                controlPoints.Add(P3);

                // Add the list of control points for the current curve segment to the list of segments
                controlPointsSegments.Add(controlPoints);
            }

            for (int i = 0; i < controlPointsSegments.Count; i++)
            {
                List<PointD> controls = controlPointsSegments[i];
                List<PointD> pointsTop = DeCasteljau.BezierCurve(controls, 10);

                var top = formsPlot1.Plot.AddScatterList(color: Color.Green, lineStyle: ScottPlot.LineStyle.Solid);

                for (int j = 0; j < pointsTop.Count; j++)
                {
                    top.Add(pointsTop[j].X, pointsTop[j].Y);
                }
            }


        }
    }
}

//implement a PointD class to replace PointD to use double instead of double
public class PointD
{
    public double X { get; set; }
    public double Y { get; set; }

    public PointD(double x, double y)
    {
        X = x;
        Y = y;
    }

    public PointD()
    {
    }
}