using BezierAirfoilDesigner.Properties;
using ScottPlot;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace BezierAirfoilDesigner
{
    public partial class Form1 : Form
    {

        //--------------------------------------------------------------------------------------------------------------------------------------
        // global variables

        static string currentVersion = "v0.9.5";

        private static DateTime currentTime;
        private static DateTime startTime;
        private static TimeSpan elapsedTime;

        string loadedAirfoilName = "";

        readonly List<PointD> defaultControlPointsTop = new()
        {
            new PointD(0, 0),
            new PointD(0, 0.1),
            new PointD(0.5, 0.1),
            new PointD(1, 0)
        };
        readonly List<PointD> defaultControlPointsBottom = new()
        {
            new PointD(0, 0),
            new PointD(0, -0.1),
            new PointD(0.5, -0.1),
            new PointD(1, 0)
        };

        List<PointD> referenceDatTop = new();
        List<PointD> referenceDatBottom = new();

        List<PointD> errorTop = new();
        List<PointD> errorBottom = new();

        List<PointD> errorOfEachControlPointTop = new();
        List<PointD> errorOfEachControlPointBottom = new();

        double totalErrorTop;
        double totalErrorBottom;
        double errorThresholdTop = 0.075;
        double errorThresholdBottom = 0.075;

        readonly double minZoomRange = 0.01;
        readonly double maxZoomRange = 250.0;

        private static bool showControlTop;
        private static bool showControlBottom;
        private static bool showTop;
        private static bool showBottom;
        private static bool showThickness;
        private static bool showCamber;
        private static bool showRadius;
        private static bool showReferenceTop;
        private static bool showReferenceBottom;
        private static bool updateUI;

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
            chkUpdateUI.Checked = true;

            chkShowReferenceTop.Enabled = false;
            chkShowReferenceBottom.Enabled = false;
            btnSearchTop.Enabled = false;
            btnSearchBottom.Enabled = false;
            btnAutoSearch.Enabled = false;

            progressBar1.Visible = false;
            //button1.Visible = false;

            cmbLanguage.Items.Add("en");
            cmbLanguage.Items.Add("de");
            cmbLanguage.SelectedIndex = 0;

            cmbErrorCalculationDistribution.Items.Add("uniform");
            cmbErrorCalculationDistribution.Items.Add("sine");
            cmbErrorCalculationDistribution.Items.Add("cosine");
            cmbErrorCalculationDistribution.SelectedIndex = 2;

            calculations();
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        // main calculations

        void calculations()
        {
            if (updateUI)
            {
                var axisLimits = formsPlot1.Plot.GetAxisLimits();
                formsPlot1.Plot.Clear();
                formsPlot1.Plot.SetAxisLimits(axisLimits);
            }

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating and plotting points on the top bezier curve

            List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);

            // Try to parse the number of points, if unsuccessful show a message box and return
            if (int.TryParse(txtNumOfPointsTop.Text, CultureInfo.InvariantCulture, out int numPointsTop) == false || numPointsTop < 2)
            {
                MessageBox.Show("Invalid number of points for the top curve.");
                return;
            }

            List<PointD> pointsTop = DeCasteljau.BezierCurve(controlPointsTop, numPointsTop);

            if (updateUI)
            {
                Color colorTop;
                if (showTop) { colorTop = Color.Red; } else { colorTop = Color.Transparent; }
                var top = formsPlot1.Plot.AddScatterList(color: colorTop, lineStyle: ScottPlot.LineStyle.Solid);

                for (int i = 0; i < pointsTop.Count; i++)
                {
                    top.Add(pointsTop[i].X, pointsTop[i].Y);
                }
            }

            lblOrderTop.Text = "order: " + (controlPointsTop.Count - 1).ToString();

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating and plotting points on the bottom bezier curve

            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);

            // Try to parse the number of points, if unsuccessful show a message box and return
            if (int.TryParse(txtNumOfPointBottom.Text, CultureInfo.InvariantCulture, out int numPointsBottom) == false || numPointsBottom < 2)
            {
                MessageBox.Show("Invalid number of points for the bottom curve.");
                return;
            }

            List<PointD> pointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numPointsBottom);

            if (updateUI)
            {
                Color colorBottom;
                if (showBottom) { colorBottom = Color.Red; } else { colorBottom = Color.Transparent; }
                var bottom = formsPlot1.Plot.AddScatterList(color: colorBottom, lineStyle: ScottPlot.LineStyle.Solid);

                for (int i = 0; i < pointsBottom.Count; i++)
                {
                    bottom.Add(pointsBottom[i].X, pointsBottom[i].Y);
                }
            }

            lblOrderBottom.Text = "order: " + (controlPointsBottom.Count - 1).ToString();

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating and plotting the thickness line

            // Try to parse the thickness step size, if unsuccessful (e.g. not numeric) set to 0.001f
            if (int.TryParse(txtThicknessStepSize.Text.Replace(",", "."), CultureInfo.InvariantCulture, out int thicknessNumStations) == false || thicknessNumStations < 0 || thicknessNumStations > 100000)
            {
                MessageBox.Show("Invalid thickness step size.");
                return;
            }

            List<PointD> thicknesses = GetThickness(pointsTop, pointsBottom, thicknessNumStations, GetCosineStations);
            PointD maxThickness = new PointD();

            for (int i = 0; i < thicknesses.Count; i++)
            {
                if (thicknesses[i].Y > maxThickness.Y)
                {
                    maxThickness = thicknesses[i];
                }
            }

            if (updateUI)
            {
                Color thicknessLineColor = new();
                if (showThickness) { thicknessLineColor = Color.Gray; } else { thicknessLineColor = Color.Transparent; }
                var thicknessLine = formsPlot1.Plot.AddScatterList(color: thicknessLineColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);
                //var thicknessStations = formsPlot1.Plot.AddScatterList(color: thicknessLineColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);

                for (int i = 0; i < thicknesses.Count; i++)
                {
                    thicknessLine.Add(thicknesses[i].X, thicknesses[i].Y);
                    //thicknessStations.Add(thicknesses[i].X, 0);
                    //thicknessStations.Add(thicknesses[i].X, thicknesses[i].Y);
                    //thicknessStations.Add(thicknesses[i].X, 0);
                }

                Color maxThicknessMarkColor = new();
                if (showThickness) { maxThicknessMarkColor = Color.Gray; } else { maxThicknessMarkColor = Color.Transparent; }
                var maxThicknessMark = formsPlot1.Plot.AddScatterList(color: maxThicknessMarkColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);
                maxThicknessMark.Add(maxThickness.X, maxThickness.Y);
                maxThicknessMark.Add(maxThickness.X, 0.0f);
            }

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating and plotting the camber line


            //try to parse the camber position, if unsucessful (eg not numeric) set to 0.5f
            if (double.TryParse(txtCamberPosition.Text.Replace(",", "."), CultureInfo.InvariantCulture, out double camberPosition) == false || camberPosition < 0.0f || camberPosition > 1.0f)
            {
                MessageBox.Show("Invalid camber position.");
                return;
            }

            //try to parse the camber step size, if unsuccessful (e.g. not numeric) set to 0.001f
            if (int.TryParse(txtCamberStepSize.Text.Replace(",", "."), CultureInfo.InvariantCulture, out int camberNumStations) == false || camberNumStations < 0 || camberNumStations > 100000)
            {
                MessageBox.Show("Invalid camber step size.");
                return;
            }

            List<PointD> camber = GetCamber(pointsTop, pointsBottom, camberPosition, camberNumStations, GetCosineStations);
            PointD maxCamber = new PointD();

            for (int i = 0; i < camber.Count; i++)
            {
                if (Math.Abs(camber[i].Y) > Math.Abs(maxCamber.Y))
                {
                    maxCamber = camber[i];
                }
            }

            if (updateUI)
            {
                Color midLineColor = new();
                if (showCamber) { midLineColor = Color.Gray; } else { midLineColor = Color.Transparent; }
                var camberLine = formsPlot1.Plot.AddScatterList(color: midLineColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);

                for (int i = 0; i < camber.Count; i++)
                {
                    camberLine.Add(camber[i].X, camber[i].Y);
                }

                Color maxCamberMarkColor = new();
                if (showCamber) { maxCamberMarkColor = Color.Gray; } else { maxCamberMarkColor = Color.Transparent; }
                var maxCamberMark = formsPlot1.Plot.AddScatterList(color: maxCamberMarkColor, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);
                maxCamberMark.Add(maxCamber.X, maxCamber.Y);
                maxCamberMark.Add(maxCamber.X, 0.0f);
            }

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating and drawing the LE radius

            PointD midpoint = CircleProperties.CalculateMidpoint(pointsBottom[1], pointsTop[0], pointsTop[1]);
            double radius = CircleProperties.CalculateRadius(pointsBottom[1], pointsTop[0], pointsTop[1]);

            if (updateUI)
            {
                Color circleColor;
                if (showRadius) { circleColor = Color.Gray; } else { circleColor = Color.Transparent; }
                formsPlot1.Plot.AddCircle(x: midpoint.X, y: midpoint.Y, radius: radius, color: circleColor, lineWidth: 1, lineStyle: ScottPlot.LineStyle.Dash);
            }

            //----------------------------------------------------------------------------------------------------------------------------------
            // printing airfoil parameters to the text box

            //txtAirfoilParam.Text = "";
            string airfoilParamText = "";
            airfoilParamText += ("nose radius:\t\t" + radius + System.Environment.NewLine);
            airfoilParamText += ("maximum camber:\t" + maxCamber.Y.ToString() + System.Environment.NewLine + "\tat:\t\t" + maxCamber.X.ToString() + System.Environment.NewLine);
            airfoilParamText += ("maximum thickness:\t" + maxThickness.Y.ToString() + System.Environment.NewLine + "\tat:\t\t" + maxThickness.X.ToString() + System.Environment.NewLine);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Plotting the control polygons of the top and bottom bezier curves

            if (updateUI)
            {
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
            }

            //----------------------------------------------------------------------------------------------------------------------------------
            // Add the reference .dat airfoil to the plot

            if (updateUI)
            {
                Color referenceTopColor = new();
                if (showReferenceTop) { referenceTopColor = Color.Blue; } else { referenceTopColor = Color.Transparent; }
                var referenceDatPlotTop = formsPlot1.Plot.AddScatterList(color: referenceTopColor, lineStyle: ScottPlot.LineStyle.Dash, lineWidth: 1, markerSize: 4);

                Color referenceBottomColor = new();
                if (showReferenceBottom) { referenceBottomColor = Color.Blue; } else { referenceBottomColor = Color.Transparent; }
                var referenceDatPlotBottom = formsPlot1.Plot.AddScatterList(color: referenceBottomColor, lineStyle: ScottPlot.LineStyle.Dash, lineWidth: 1, markerSize: 4);


                //double[] xsTop = new double[referenceDatTop.Count];
                //double[] ysTop = new double[referenceDatTop.Count];
                //double[] xsBottom = new double[referenceDatTop.Count];
                //double[] ysBottom = new double[referenceDatTop.Count];

                for (int i = 0; i < referenceDatTop.Count; i++)
                {
                    referenceDatPlotTop.Add(referenceDatTop[i].X, referenceDatTop[i].Y);
                    //xsTop[i] = referenceDatTop[i].X;
                    //ysTop[i] = referenceDatTop[i].Y;
                }
                for (int i = 0; i < referenceDatBottom.Count; i++)
                {
                    referenceDatPlotBottom.Add(referenceDatBottom[i].X, referenceDatBottom[i].Y);
                    //xsBottom[i] = referenceDatBottom[i].X;
                    //ysBottom[i] = referenceDatBottom[i].Y;
                }

                //check if xsTop is empty
                //if (xsTop.Length != 0)
                //{
                //    (double[] smoothXsTop, double[] smoothYsTop) = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(xsTop, ysTop, 200);
                //    (double[] smoothXsBottom, double[] smoothYsBottom) = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(xsBottom, ysBottom, 200);

                //    formsPlot1.Plot.AddScatter(smoothXsTop, smoothYsTop);
                //    formsPlot1.Plot.AddScatter(smoothXsBottom, smoothYsBottom);
                //}

            }

            //----------------------------------------------------------------------------------------------------------------------------------
            // calculating the error between the bezier airfoil and the reference airfoil and writing results to the text field

            errorTop.Clear();

            int errorCalculationDistribution = cmbErrorCalculationDistribution.SelectedIndex;

            switch (errorCalculationDistribution)
            {
                case 0:
                    errorTop = GetThickness(pointsTop, referenceDatTop, 1000, GetUniformStations);
                    break;
                case 1:
                    errorTop = GetThickness(pointsTop, referenceDatTop, 1000, GetSineStations);
                    break;
                case 2:
                    errorTop = GetThickness(pointsTop, referenceDatTop, 1000, GetCosineStations);
                    break;
                default:
                    errorTop = GetThickness(pointsTop, referenceDatTop, 1000, GetUniformStations);
                    break;
            }

            //errorTop = GetThickness(pointsTop, referenceDatTop, 1000, GetCosineStations);
            //totalErrorTop = CalculateAreaUnderCurve(errorTop) * 1000;

            totalErrorTop = 0;

            for (int i = 0; i < errorTop.Count; i++)
            {
                totalErrorTop += Math.Abs(errorTop[i].Y);
            }


            errorOfEachControlPointTop.Clear();

            for (int i = 0; i < controlPointsTop.Count; i++)
            {
                double targetX = controlPointsTop[i].X;  // The x-value you're interested in
                double delta = 1 / (double)(controlPointsTop.Count - 1);  // The range around the x-value

                double ySum = errorTop.Where(point => Math.Abs(point.X - targetX) <= delta).Sum(point => point.Y);

                errorOfEachControlPointTop.Add(new PointD(targetX, ySum));
            }


            errorBottom.Clear();
            switch (errorCalculationDistribution)
            {
                case 0:
                    errorBottom = GetThickness(pointsBottom, referenceDatBottom, 1000, GetUniformStations);
                    break;
                case 1:
                    errorBottom = GetThickness(pointsBottom, referenceDatBottom, 1000, GetSineStations);
                    break;
                case 2:
                    errorBottom = GetThickness(pointsBottom, referenceDatBottom, 1000, GetCosineStations);
                    break;
                default:
                    errorBottom = GetThickness(pointsBottom, referenceDatBottom, 1000, GetUniformStations);
                    break;
            }

            //totalErrorBottom = CalculateAreaUnderCurve(errorBottom) * 1000;

            totalErrorBottom = 0;

            for (int i = 0; i < errorBottom.Count; i++)
            {
                totalErrorBottom += Math.Abs(errorBottom[i].Y);
            }


            errorOfEachControlPointBottom.Clear();

            for (int i = 0; i < controlPointsBottom.Count; i++)
            {
                double targetX = controlPointsBottom[i].X;  // The x-value you're interested in
                double delta = 1 / (double)(controlPointsBottom.Count - 1);  // The range around the x-value

                double ySum = errorBottom.Where(point => Math.Abs(point.X - targetX) <= delta).Sum(point => point.Y);

                errorOfEachControlPointBottom.Add(new PointD(targetX, ySum));
            }

            if (totalErrorTop > 0) { airfoilParamText += (System.Environment.NewLine + "error top:\t\t" + totalErrorTop + System.Environment.NewLine); }
            if (totalErrorBottom > 0) { airfoilParamText += ("error bottom:\t\t" + totalErrorBottom + System.Environment.NewLine); }

            // Append each control point error to txtAirfoilParam
            //for (int i = 0; i < errorOfEachControlPointTop.Count; i++)
            //{
            //    txtAirfoilParam.AppendText($"Error at control point {i} (x={errorOfEachControlPointTop[i].X}): {errorOfEachControlPointTop[i].Y}" + System.Environment.NewLine);
            //}

            //----------------------------------------------------------------------------------------------------------------------------------

            txtAirfoilParam.Text = airfoilParamText;

            if (updateUI) { formsPlot1.Refresh(); }
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
                "btnDecreaseOrderBottom", "txtNumOfPointBottom", "btnAxisAuto", "txtChord",
                "btnStartPSOTop", "btnStartPSOBottom"
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
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        // Particle Swarm Optimization

        public class Particle
        {
            public double[] Position { get; set; }
            public double[] Velocity { get; set; }
            public double[] BestPosition { get; set; }
            public double BestScore { get; set; }
        }

        public int ParticleCount { get; set; } = 100;
        public double Inertia { get; set; } = 0.5;
        public double PersonalAcceleration { get; set; } = 0.1;
        public double GlobalAcceleration { get; set; } = 0.1;
        public int MaxIterations { get; set; } = 1000;
        public double[] GlobalBestPosition { get; set; }
        public double GlobalBestScore { get; set; } = double.MaxValue; // Assuming minimization problem

        public void ResetGlobalBests()
        {
            GlobalBestPosition = null; // Clearing the array
            GlobalBestScore = double.MaxValue; // Resetting to initial value
        }


        public int StalledIterations { get; set; } = 0;
        public int MaxStalledIterations { get; set; } = 50;  // Adjust based on your preference
        public double TerminationThreshold { get; set; } = 0.01;  // Adjust based on the precision you need



        private Particle[] particles;

        public void Initialize(double[] initialControlPoints)
        {
            particles = new Particle[ParticleCount];

            for (int i = 0; i < ParticleCount; i++)
            {
                particles[i] = new Particle
                {
                    Position = (double[])initialControlPoints.Clone(),
                    Velocity = new double[initialControlPoints.Length],
                    BestPosition = new double[initialControlPoints.Length],
                    BestScore = double.MaxValue
                };

                // Set velocities for the first control point to 0
                particles[i].Velocity[0] = 0;  // x of controlPoint[0]
                particles[i].Velocity[1] = 0;  // y of controlPoint[0]

                particles[i].Velocity[2] = 0;  // x of controlPoint[1]

                // Only vary the y-value of the second control point
                for (int j = 3; j < initialControlPoints.Length - 2; j++)
                {
                    double positionVariationMultiplier = (new Random().NextDouble() * 0.5) * 0.25;
                    double variationY = (new Random().NextDouble() * positionVariationMultiplier) - positionVariationMultiplier / 2;
                    particles[i].Position[j] += variationY;  // y of controlPoint[j]
                }

                for (int j = 3; j < initialControlPoints.Length - 2; j++)
                {
                    double velocityVariationMultiplier = (new Random().NextDouble() * 0.5) - 0.25;
                    // Initialize velocities for the other control points
                    particles[i].Velocity[j] = (new Random().NextDouble() * velocityVariationMultiplier) - velocityVariationMultiplier / 2;
                }
            }
        }




        public void Optimize(Func<double[], double> objectiveFunction)
        {
            cancelSearch = false;

            double previousGlobalBestScore = GlobalBestScore;
            int stalledIterations = 0;
            const int MaxStalledIterations = 50;  // Adjust based on your preference
            const double TerminationThreshold = 0.001;  // Adjust based on the precision you need


            for (int iteration = 0; iteration < MaxIterations; iteration++)
            {
                if (cancelSearch)
                {
                    MessageBox.Show("Terminated at iteration: " + iteration);
                    break;  // Exit the loop
                }

                this.Invoke((MethodInvoker)delegate
                {
                    currentTime = DateTime.Now;
                    elapsedTime = currentTime - startTime;
                    System.Windows.Forms.Control control = panel3.Controls.Find("lblElapsedTime", true)[0];
                    control.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
                    control.Refresh();
                });

                foreach (Particle particle in particles)
                {
                    // Evaluate current fitness
                    double currentScore = objectiveFunction(particle.Position);

                    // Update personal best
                    if (currentScore < particle.BestScore)
                    {
                        particle.BestScore = currentScore;
                        particle.BestPosition = (double[])particle.Position.Clone();
                    }

                    // Update global best
                    if (currentScore < GlobalBestScore)
                    {
                        GlobalBestScore = currentScore;
                        GlobalBestPosition = (double[])particle.Position.Clone();
                    }
                }


                // Update velocities and positions
                foreach (Particle particle in particles)
                {
                    Inertia = new Random().NextDouble();
                    PersonalAcceleration = new Random().NextDouble();
                    GlobalAcceleration = new Random().NextDouble();

                    for (int j = 0; j < particle.Position.Length; j++)
                    {
                        double r1 = new Random().NextDouble();
                        double r2 = new Random().NextDouble();

                        if (j == particle.Position.Length - 1 || j == particle.Position.Length - 2)
                        {
                            // Skip updating the last control point (both x and y)
                            continue;
                        }
                        else if (j == 0 || j == 1)
                        {
                            // Skip updating the first control point (both x and y)
                            continue;
                        }
                        else if (j == 2)
                        {
                            // Skip updating the x-coordinate of the second control point
                            continue;
                        }
                        //else if (j == 3)
                        //{
                        //    // Only update the y-coordinate of the second control point
                        //    particle.Velocity[j] = Inertia * particle.Velocity[j]
                        //                           + PersonalAcceleration * r1 * (particle.BestPosition[j] - particle.Position[j])
                        //                           + GlobalAcceleration * r2 * (GlobalBestPosition[j] - particle.Position[j]);
                        //    particle.Position[j] += particle.Velocity[j];
                        //}
                        else
                        {
                            // Update all other control points normally
                            particle.Velocity[j] = Inertia * particle.Velocity[j]
                                                   + PersonalAcceleration * r1 * (particle.BestPosition[j] - particle.Position[j])
                                                   + GlobalAcceleration * r2 * (GlobalBestPosition[j] - particle.Position[j]);

                            particle.Position[j] += particle.Velocity[j];
                        }
                    }
                }

                if (Math.Abs(GlobalBestScore - previousGlobalBestScore) < TerminationThreshold)
                {
                    stalledIterations++;
                    if (stalledIterations >= MaxStalledIterations)
                    {
                        Console.Beep();
                        Console.Beep();
                        Console.Beep();

                        //show message box with number of iterations, start time, end time, and elapsed time
                        MessageBox.Show("Terminated at iteration: " + iteration + $"\nStart time: {startTime}\nEnd time: {currentTime}\nElapsed time: {elapsedTime}");
                        break;  // Exit the loop
                    }
                }
                else
                {
                    stalledIterations = 0;  // Reset if there's a significant change
                }

                previousGlobalBestScore = GlobalBestScore;
            }
        }

        private double[] ConvertControlPointsToArray(List<PointD> controlPoints)
        {
            double[] array = new double[controlPoints.Count * 2];
            int index = 0;
            foreach (var point in controlPoints)
            {
                array[index++] = point.X;
                array[index++] = point.Y;
            }
            return array;
        }

        private List<PointD> ConvertArrayToControlPoints(double[] array)
        {
            List<PointD> controlPoints = new List<PointD>();
            for (int i = 0; i < array.Length; i += 2)
            {
                controlPoints.Add(new PointD(array[i], array[i + 1]));
            }
            return controlPoints;
        }

        //private double ObjectiveFunction(double[] controlPointsArray, bool topOrBottom)
        //{
        //    DataGridView gridView = topOrBottom ? dataGridViewTop : dataGridViewBottom;
        //    double totalError = topOrBottom ? totalErrorTop : totalErrorBottom;

        //    List<PointD> controlPoints = ConvertArrayToControlPoints(controlPointsArray);
        //    gridViewAddPoints(gridView, controlPoints);

        //    this.Invoke((MethodInvoker)delegate
        //    {
        //        // UI update code goes here
        //        calculations();
        //    });

        //    return totalError;
        //}

        //private async Task StartPSO(bool topOrBottom)
        //{
        //    DataGridView gridView = topOrBottom ? dataGridViewTop : dataGridViewBottom;
        //    Func<double[], double> objectiveFunction = (controlPointsArray) => ObjectiveFunction(controlPointsArray, topOrBottom);

        //    var controlPoints = GetControlPoints(gridView);
        //    var initialControlPointsArray = ConvertControlPointsToArray(controlPoints);

        //    Initialize(initialControlPointsArray);
        //    await Task.Run(() => Optimize(objectiveFunction));

        //    var optimizedControlPoints = ConvertArrayToControlPoints(GlobalBestPosition);
        //    gridViewAddPoints(gridView, optimizedControlPoints);
        //}

        private double ObjectiveFunctionTop(double[] controlPointsArray)
        {
            List<PointD> controlPoints = ConvertArrayToControlPoints(controlPointsArray);
            gridViewAddPoints(dataGridViewTop, controlPoints);

            this.Invoke((MethodInvoker)delegate
            {
                // UI update code goes here
                calculations();
            });

            return totalErrorTop;
        }

        private async Task StartPSOTop()
        {
            ResetGlobalBests();

            var controlPoints = GetControlPoints(dataGridViewTop);
            var initialControlPointsArray = ConvertControlPointsToArray(controlPoints);

            Initialize(initialControlPointsArray);
            await Task.Run(() => Optimize(ObjectiveFunctionTop));

            var optimizedControlPoints = ConvertArrayToControlPoints(GlobalBestPosition);
            gridViewAddPoints(dataGridViewTop, optimizedControlPoints);
        }

        private double ObjectiveFunctionBottom(double[] controlPointsArray)
        {
            List<PointD> controlPoints = ConvertArrayToControlPoints(controlPointsArray);
            gridViewAddPoints(dataGridViewBottom, controlPoints);

            this.Invoke((MethodInvoker)delegate
            {
                // UI update code goes here
                calculations();
            });

            return totalErrorBottom;
        }

        private async Task StartPSOBottom()
        {
            ResetGlobalBests();

            var controlPoints = GetControlPoints(dataGridViewBottom);
            var initialControlPointsArray = ConvertControlPointsToArray(controlPoints);

            Initialize(initialControlPointsArray);
            await Task.Run(() => Optimize(ObjectiveFunctionBottom));

            var optimizedControlPoints = ConvertArrayToControlPoints(GlobalBestPosition);
            gridViewAddPoints(dataGridViewBottom, optimizedControlPoints);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        // helper functions

        private async Task SearchTopAsync(bool singleSearch)
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

                // If single search, then break after one iteration
                if (singleSearch) break;
            }

            progressBar1.Visible = false;
        }

        private async Task SearchBottomAsync(bool singleSearch)
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

                // If single search, then break after one iteration
                if (singleSearch) break;
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
                    if (currentLowestError < (topOrBottom ? errorThresholdTop : errorThresholdBottom))
                    {
                        betterCombinationFound = true;
                        return;
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
                _ = double.TryParse(s: gridView.Rows[i].Cells[0].Value.ToString().Replace(",", "."), CultureInfo.InvariantCulture, out double x);
                _ = double.TryParse(s: gridView.Rows[i].Cells[1].Value.ToString().Replace(",", "."), CultureInfo.InvariantCulture, out double y);
                // Create a PointD object
                PointD point = new(x, y);
                controlPointsBottom.Add(point);
            }

            return controlPointsBottom;
        }


        private static List<double> GetUniformStations(int numberOfStations)
        {
            return Enumerable.Range(0, numberOfStations).Select(i => (double)i / (numberOfStations - 1)).ToList();
        }

        private static List<double> GetSineStations(int numberOfStations)
        {
            List<double> stations = new();
            for (int i = 0; i < numberOfStations; i++)
            {
                double x = (double)i / (numberOfStations - 1); // Normalize x to [0, 1]
                double stationPosition = x * Math.Sin(x * Math.PI / 2);
                stations.Add(stationPosition);
            }
            return stations;
        }

        private static List<double> GetCosineStations(int numberOfStations)
        {
            List<double> stations = new();
            for (int i = 0; i < numberOfStations; i++)
            {
                double x = (double)i / (numberOfStations - 1); // Normalize x to [0, 1]
                double stationPosition = (Math.Cos(Math.PI * (1 - x)) + 1) / 2;
                stations.Add(stationPosition);
            }
            return stations;
        }

        private static List<PointD> GetThickness(List<PointD> curve1, List<PointD> curve2, int numberOfStations, Func<int, List<double>> distributionMethod)
        {
            List<PointD> distances = new();

            if (!curve1.Any() || !curve2.Any()) { return distances; }

            // Ensure the points are sorted by X in ascending order.
            curve1 = curve1.OrderBy(p => p.X).ToList();
            curve2 = curve2.OrderBy(p => p.X).ToList();

            // Get the station positions using the specified distribution method
            List<double> stations = distributionMethod(numberOfStations);

            foreach (double station in stations)
            {
                double x = station * (curve2.Last().X - curve1.First().X) + curve1.First().X;
                double? y1 = InterpolateY(x, curve1);
                double? y2 = InterpolateY(x, curve2);
                if (y1.HasValue && y2.HasValue)
                    distances.Add(new PointD(x, Math.Abs(y1.Value - y2.Value)));
            }

            return distances;
        }

        private static List<PointD> GetCamber(List<PointD> curve1, List<PointD> curve2, double camberPosition, int numberOfStations, Func<int, List<double>> distributionMethod)
        {
            // Ensure the points are sorted by X in ascending order.
            curve1 = curve1.OrderBy(p => p.X).ToList();
            curve2 = curve2.OrderBy(p => p.X).ToList();

            // Get the station positions using the specified distribution method
            List<double> stations = distributionMethod(numberOfStations);

            // The X range over which we calculate distances.
            double minX = Math.Max(curve1.First().X, curve2.First().X);
            double maxX = Math.Min(curve1.Last().X, curve2.Last().X);

            // Calculate midpoints at specified stations within this range.
            List<PointD> midpoints = new();
            foreach (double station in stations)
            {
                double x = station * (maxX - minX) + minX;
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

            // Now `midpoints` contains the midpoints between the curves at the specified stations,
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
                panel3.Height = activeForm.Height - lblTop.Top - (activeForm.Height - (txtAirfoilParam.Top + txtAirfoilParam.Height));

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

        private void chkUpdateUI_CheckedChanged(object sender, EventArgs e)
        {
            updateUI = chkUpdateUI.Checked;
            calculations();
        }

        private void cmbErrorCalculationDistribution_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void txtErrorThresholdTop_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtErrorThresholdTop.Text.Replace(",", "."), CultureInfo.InvariantCulture, out errorThresholdTop) == false)
            {
                MessageBox.Show("Invalid Error Threshold Top.");
                return;
            }
        }

        private void txtErrorThresholdBottom_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtErrorThresholdBottom.Text.Replace(",", "."), CultureInfo.InvariantCulture, out errorThresholdBottom) == false)
            {
                MessageBox.Show("Invalid Error Threshold Bottom.");
                return;
            }
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
            if (!ValidateDegreeCompatibility(controlPointsTop)) return;
            controlPointsTop = DeCasteljau.IncreaseOrder(controlPointsTop);
            gridViewAddPoints(dataGridViewTop, controlPointsTop);
            calculations();
        }

        private void btnIncreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);
            if (!ValidateDegreeCompatibility(controlPointsBottom)) return;
            controlPointsBottom = DeCasteljau.IncreaseOrder(controlPointsBottom);
            gridViewAddPoints(dataGridViewBottom, controlPointsBottom);
            calculations();
        }

        private bool ValidateDegreeCompatibility(List<PointD> controlPoints)
        {
            if (controlPoints.Count != 10 && controlPoints.Count != 11)
            {
                return true;
            }

            string warningMessage = "For optimal compatibility with most CAD software, increasing the degree above 9 is discouraged. DXF export supports up to degree 10 only. Do you wish to continue?";
            DialogResult dialogResult = MessageBox.Show(warningMessage, "⚠ Compatibility Warning", MessageBoxButtons.YesNo);

            return dialogResult != DialogResult.No;
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

                while ((line = reader.ReadLine()) != null)
                {
                    // split the line on whitespace
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 2)
                    {
                        // parse the parts as doubles
                        if (double.TryParse(parts[0].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double x)
                        && double.TryParse(parts[1].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
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

            if (chkMatchTEGap.Checked == true)
            {
                List<PointD> controlPointsTop = GetControlPoints(dataGridViewTop);
                controlPointsTop[controlPointsTop.Count - 1] = referenceDatTop[referenceDatTop.Count - 1];
                gridViewAddPoints(dataGridViewTop, controlPointsTop);

                List<PointD> controlPointsBottom = GetControlPoints(dataGridViewBottom);
                controlPointsBottom[controlPointsBottom.Count - 1] = referenceDatBottom[referenceDatBottom.Count - 1];
                gridViewAddPoints(dataGridViewBottom, controlPointsBottom);
            }

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

                // header line is written to the global variable, for use in saving
                var line = reader.ReadLine();
                if (line != null)
                {
                    loadedAirfoilName = line;
                }

                while ((line = reader.ReadLine()) != null)
                {
                    // split the line on whitespace
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 2)
                    {
                        // parse the parts as doubles
                        if (double.TryParse(parts[0].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double x)
                        && double.TryParse(parts[1].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
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

                // header line is written to the global variable, for use in saving
                var line = reader.ReadLine();
                if (line != null)
                {
                    loadedAirfoilName = line;
                }

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
                        if (double.TryParse(parts[0].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double x)
                        && double.TryParse(parts[1].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
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

            await SearchTopAsync(false);

            btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
        }

        private async void btnSearchTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;
                cancelSearch = false;
                startTime = DateTime.Now;
                await SearchTopAsync(true);
                btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
            }
        }

        private async void btnSearchBottom_Click(object sender, EventArgs e)
        {
            btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;

            cancelSearch = false;

            startTime = DateTime.Now;

            await SearchBottomAsync(false);

            btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
        }
        private async void btnSearchBottom_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;
                cancelSearch = false;
                startTime = DateTime.Now;
                await SearchBottomAsync(true);
                btnSearchTop.Enabled = true; btnSearchBottom.Enabled = true; btnAutoSearch.Enabled = true;
            }
        }

        private async void btnAutoSearch_Click(object sender, EventArgs e)
        {
            btnSearchTop.Enabled = false; btnSearchBottom.Enabled = false; btnAutoSearch.Enabled = false;
            cancelSearch = false;

            // Record the start time
            startTime = DateTime.Now;

            double previousError = totalErrorTop + totalErrorBottom;
            double currentError;
            double errorImprovement = 1.0f;

            await SearchTopAsync(false);
            await SearchBottomAsync(false);

            while (totalErrorTop > errorThresholdTop || totalErrorBottom > errorThresholdBottom)
            {
                // Check if the operation should be cancelled
                if (cancelSearch) break;

                if (totalErrorTop >= errorThresholdTop)
                {
                    btnIncreaseOrderTop.PerformClick();
                }

                if (totalErrorBottom >= errorThresholdBottom)
                {
                    btnIncreaseOrderBottom.PerformClick();
                }

                await SearchTopAsync(false);
                await SearchBottomAsync(false);

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            frmSave saveForm = new frmSave();

            // Set properties
            saveForm.controlPointsTop = GetControlPoints(dataGridViewTop); ;     // Your data for controlPointsTop
            saveForm.controlPointsBottom = GetControlPoints(dataGridViewBottom); ;  // Your data for controlPointsBottom

            if (int.TryParse(txtNumOfPointsTop.Text, CultureInfo.InvariantCulture, out int numPointsTop) == false || numPointsTop < 2)
            {
                MessageBox.Show("Invalid number of points for the top curve.");
                return;
            }

            if (int.TryParse(txtNumOfPointBottom.Text, CultureInfo.InvariantCulture, out int numPointsBottom) == false || numPointsTop < 2)
            {
                MessageBox.Show("Invalid number of points for the top curve.");
                return;
            }

            saveForm.pointsTop = DeCasteljau.BezierCurve(GetControlPoints(dataGridViewTop), numPointsTop);            // Your data for pointsTop
            saveForm.pointsBottom = DeCasteljau.BezierCurve(GetControlPoints(dataGridViewBottom), numPointsBottom);        // Your data for pointsBottom

            saveForm.loadedAirfoilName = loadedAirfoilName;    // Your data for loadedAirfoilName

            saveForm.ShowDialog();
        }

        private async void btnStartPSOTop_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now;
            await StartPSOTop(/*true*/); // true for top
        }

        private async void btnStartPSOBottom_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now;
            await StartPSOBottom(/*false*/); // false for bottom
        }
    }
}

//implement a PointD class to replace PointF to use double instead of float
public class PointD : IEquatable<PointD>
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

    public bool Equals(PointD other)
    {
        if (other == null)
            return false;

        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        return Equals((PointD)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}