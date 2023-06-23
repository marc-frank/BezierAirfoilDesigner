//using MathNet.Numerics.LinearAlgebra;
//using MathNet.Numerics.LinearAlgebra.Double;
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
            calculations();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            formsPlot1.SetBounds(0, 0, Form1.ActiveForm.Width - 611, Form1.ActiveForm.Height - 60);
            //richTextBox1.SetBounds(Form1.ActiveForm.Width - 234, 31, 204, Form1.ActiveForm.Height - 88);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            formsPlotSettings();
            gridViewSettings();
            addDefaultPointsTop();
            addDefaultPointsBottom();
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

            //--------------------------------------------------------------------------------------------------

            for (int i = 0; i <= controlPointsTop.Count - 1; i++)
            {
                richTextBox1.AppendText(controlPointsTop[i].X.ToString() + " " + controlPointsTop[i].Y.ToString() + "\n");
            }

            richTextBox1.AppendText("\n");

            for (int i = 0; i <= controlPointsBottom.Count - 1; i++)
            {
                richTextBox1.AppendText(controlPointsBottom[i].X.ToString() + " " + controlPointsBottom[i].Y.ToString() + "\n");
            }

            richTextBox1.AppendText("\n");

            //--------------------------------------------------------------------------------------------------

            richTextBox1.Text = "" + "Airfoil Name\n";

            for (int i = pointsTop.Count - 1; i >= 0; i--)
            {
                richTextBox1.AppendText($"{pointsTop[i].X:0.00000000} {pointsTop[i].Y:0.00000000}" + "\n");
            }

            for (int i = 1; i <= pointsBottom.Count - 1; i++)
            {
                richTextBox1.AppendText($"{pointsBottom[i].X:0.00000000} {pointsBottom[i].Y:0.00000000}" + "\n");
            }

            richTextBox1.Text = richTextBox1.Text.Replace(',', '.');

            //--------------------------------------------------------------------------------------------------

            formsPlot1.Plot.Clear();

            var topControl = formsPlot1.Plot.AddScatterList(color: Color.Black, lineStyle: ScottPlot.LineStyle.Dash);
            var bottomControl = formsPlot1.Plot.AddScatterList(color: Color.Black, lineStyle: ScottPlot.LineStyle.Dash);

            for (int i = 0; i < controlPointsTop.Count; i++)
            {
                topControl.Add(controlPointsTop[i].X, controlPointsTop[i].Y);
            }
            for (int i = 0; i < controlPointsBottom.Count; i++)
            {
                bottomControl.Add(controlPointsBottom[i].X, controlPointsBottom[i].Y);
            }

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

            PointF maxCamber = new PointF();
            int maxCamberIndex = 0;
            PointF maxThickness = new PointF();
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

        private void formsPlotSettings()
        {
            formsPlot1.Plot.AxisScaleLock(enable: true, scaleMode: ScottPlot.EqualScaleMode.PreserveX);
        }
        private void gridViewSettings()
        {
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToResizeRows = false;
        }
        private void addDefaultPointsTop()
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
        private void addDefaultPointsBottom()
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
        private void gridViewAddPoints(DataGridView dataGridView, List<PointF> pointFs)
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
        private List<PointF> GetControlPoints(DataGridView gridView)
        {
            List<PointF> controlPointsBottom = new List<PointF>();

            for (int i = 0; i < gridView.Rows.Count - 1; i++)
            {
                // Retrieve the values from the DataGridView
                float x = float.Parse(gridView.Rows[i].Cells[0].Value.ToString());
                float y = float.Parse(gridView.Rows[i].Cells[1].Value.ToString());
                // Create a PointF object
                PointF point = new PointF(x, y);
                controlPointsBottom.Add(point);
            }

            return controlPointsBottom;
        }

    }
}