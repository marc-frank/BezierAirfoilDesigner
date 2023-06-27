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

        private void button1_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);

            if (int.Parse(textBox1.Text) < 3) { textBox1.Text = "3"; }
            if (int.Parse(textBox2.Text) < 3) { textBox2.Text = "3"; }

            int numPointsTop = int.Parse(textBox1.Text);
            int numPointsBottom = int.Parse(textBox2.Text);

            List<PointF> pointsTop = DeCasteljau.BezierCurve(controlPointsTop, numPointsTop);
            List<PointF> pointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numPointsBottom);

            richTextBox1.Text = "" + "Airfoil Name\n";

            for (int i = pointsTop.Count - 1; i >= 0; i--)
            {
                richTextBox1.AppendText($"{pointsTop[i].X:N8} {pointsTop[i].Y:N8}" + "\n");
            }

            for (int i = 1; i <= pointsBottom.Count - 1; i++)
            {
                richTextBox1.AppendText($"{pointsBottom[i].X:N8} {pointsBottom[i].Y:N8}" + "\n");
            }

            richTextBox1.Text = richTextBox1.Text.Replace(',', '.');
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            formsPlot1.SetBounds(0, 0, Form1.ActiveForm.Width - 611, Form1.ActiveForm.Height - 60);
            //richTextBox1.SetBounds(Form1.ActiveForm.Width - 234, 31, 204, Form1.ActiveForm.Height - 88);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormsPlotSettings();
            GridViewSettings();
            AddDefaultPointsTop();
            AddDefaultPointsBottom();
            calculations();
        }

        void calculations()
        {

            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);

            if (int.Parse(textBox1.Text) < 3) { textBox1.Text = "3"; }
            if (int.Parse(textBox2.Text) < 3) { textBox2.Text = "3"; }

            int numPointsTop = int.Parse(textBox1.Text);
            int numPointsBottom = int.Parse(textBox2.Text);

            List<PointF> pointsTop = DeCasteljau.BezierCurve(controlPointsTop, numPointsTop);
            List<PointF> pointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numPointsBottom);

            formsPlot1.Plot.Clear();

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

            var midLine = formsPlot1.Plot.AddScatterList(color: Color.Gray, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);
            var thicknessLine = formsPlot1.Plot.AddScatterList(color: Color.Gray, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);

            PointF maxCamber = new();
            int maxCamberIndex = 0;
            PointF maxThickness = new();
            int maxThicknessIndex = 0;

            for (int i = 0; i < pointsTop.Count; i++)
            {
                float mid = (pointsTop[i].Y + pointsBottom[i].Y) / 2;
                float thickness = Math.Abs(pointsTop[i].Y) + Math.Abs(pointsBottom[i].Y);

                if (mid > maxCamber.Y)
                {
                    maxCamber.X = pointsTop[i].X;
                    maxCamber.Y = mid;
                    maxCamberIndex = i;
                }

                if (thickness > maxThickness.Y)
                {
                    maxThickness.X = pointsTop[i].X;
                    maxThickness.Y = thickness;
                    maxThicknessIndex = i;
                }

                midLine.Add(pointsTop[i].X, mid);
                thicknessLine.Add(pointsTop[i].X, thickness);
            }

            var maxCamberMark = formsPlot1.Plot.AddScatterList(color: Color.Black, lineStyle: ScottPlot.LineStyle.Solid, markerSize: 0, lineWidth: 2);
            var maxThicknessMark = formsPlot1.Plot.AddScatterList(color: Color.Gray, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);

            maxCamberMark.Add(maxCamber.X, 0);
            maxCamberMark.Add(maxCamber.X, maxCamber.Y);

            maxThicknessMark.Add(pointsTop[maxThicknessIndex].X, pointsTop[maxThicknessIndex].Y);
            maxThicknessMark.Add(pointsBottom[maxThicknessIndex].X, pointsBottom[maxThicknessIndex].Y);

            PointF midpoint = CircleProperties.CalculateMidpoint(pointsBottom[1], pointsTop[0], pointsTop[1]);
            double radius = CircleProperties.CalculateRadius(pointsBottom[1], pointsTop[0], pointsTop[1]);

            richTextBox2.Text = "";
            richTextBox2.AppendText("nose radius: " + radius + System.Environment.NewLine);
            richTextBox2.AppendText("maximum camber: " + maxCamber.Y.ToString() + " @: " + maxCamber.X.ToString() + System.Environment.NewLine);
            richTextBox2.AppendText("maximum thickness: " + maxThickness.Y.ToString() + " @: " + maxThickness.X.ToString() + System.Environment.NewLine);



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
            controlTop.Color = Color.Black;
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
            controlBottom.Color = Color.Black;
            controlBottom.MarkerSize = 5;
            formsPlot1.Plot.Add(controlBottom);

            //(double[] bzX, double[] bzY) = ScottPlot.Statistics.Interpolation.Bezier.InterpolateXY(x, y, .005);
            //formsPlot1.Plot.AddScatterLines(bzX, bzY, lineWidth: 2, label: $"Bezier");

            formsPlot1.Plot.AddCircle(x: midpoint.X, y: midpoint.Y, radius: radius, color: Color.Gray, lineWidth: 1, lineStyle: ScottPlot.LineStyle.Dash);
            formsPlot1.Refresh();
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

        private void FormsPlotSettings()
        {
            formsPlot1.Plot.AxisScaleLock(enable: true, scaleMode: ScottPlot.EqualScaleMode.PreserveX);
            formsPlot1.Configuration.RightClickDragZoom = false;


        }
        private void GridViewSettings()
        {
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToResizeRows = false;
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
        private static void gridViewAddPoints(DataGridView dataGridView, List<PointF> pointFs)
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
            dataGridView.Columns.Add("xVal", "X");
            dataGridView.Columns.Add("yVal", "Y");
            for (int i = 0; i < pointFs.Count; i++)
            {
                dataGridView.Rows.Add(pointFs[i].X, pointFs[i].Y);
            }
        }
        private static List<PointF> GetControlPoints(DataGridView gridView)
        {
            List<PointF> controlPointsBottom = new();

            for (int i = 0; i < gridView.Rows.Count - 1; i++)
            {
                // Retrieve the values from the DataGridView
                float x = float.Parse(s: gridView.Rows[i].Cells[0].Value.ToString());
                float y = float.Parse(s: gridView.Rows[i].Cells[1].Value.ToString());
                // Create a PointF object
                PointF point = new(x, y);
                controlPointsBottom.Add(point);
            }

            return controlPointsBottom;
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

        private static float GetDistanceBetweenPoints(PointF pointA, PointF pointB)
        {
            double distanceX = pointA.X - pointB.X;
            double distanceY = pointA.Y - pointB.Y;
            float distance = float.Parse((Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2))).ToString());
            return distance;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            AddDefaultPointsTop();
            AddDefaultPointsBottom();
            calculations();
        }
    }
}