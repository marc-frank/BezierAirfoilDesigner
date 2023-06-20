using System.Windows.Forms;

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
            //formsPlot1.SetBounds(12, 10, Form1.ActiveForm.Width - 527, Form1.ActiveForm.Height - 67);
            //richTextBox1.SetBounds(Form1.ActiveForm.Width - 234, 31, 204, Form1.ActiveForm.Height - 88);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToResizeRows = false;

            dataGridView1.Columns.Add("xVal", "X");
            dataGridView1.Columns.Add("yVal", "Y");
            dataGridView1.Rows.Add(0, 0);
            dataGridView1.Rows.Add(0, 0.15);
            dataGridView1.Rows.Add(0.5, 0.15);
            dataGridView1.Rows.Add(1.0, 0.0);

            dataGridView2.Columns.Add("xVal", "X");
            dataGridView2.Columns.Add("yVal", "Y");
            dataGridView2.Rows.Add(0, 0);
            dataGridView2.Rows.Add(0, -0.1);
            dataGridView2.Rows.Add(0.5, -0.1);
            dataGridView2.Rows.Add(1.0, 0.0);

            calculations();
        }

        void calculations()
        {

            List<PointF> controlPointsTop = new List<PointF>();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                // Retrieve the values from the DataGridView
                float xt = float.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                float yt = float.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                // Create a PointF object
                PointF pointT = new PointF(xt, yt);
                controlPointsTop.Add(pointT);
            }



            List<PointF> controlPointsBottom = new List<PointF>();
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                // Retrieve the values from the DataGridView
                float xb = float.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString());
                float yb = float.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString());
                // Create a PointF object
                PointF pointB = new PointF(xb, yb);
                controlPointsBottom.Add(pointB);
            }


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

            formsPlot1.Plot.AxisScaleLock(enable: true, scaleMode: ScottPlot.EqualScaleMode.PreserveX);
            formsPlot1.Refresh();
        }

        public class DeCasteljau
        {
            // Calculate a point on a Bezier curve using the de Casteljau algorithm
            private static PointF DeCasteljauPoint(List<PointF> controlPoints, double t)
            {
                List<PointF> points = new List<PointF>(controlPoints);
                int n = points.Count;

                for (int r = 1; r < n; r++)
                {
                    for (int i = 0; i < n - r; i++)
                    {
                        float x = (1 - (float)t) * points[i].X + (float)t * points[i + 1].X;
                        float y = (1 - (float)t) * points[i].Y + (float)t * points[i + 1].Y;
                        points[i] = new PointF(x, y);
                    }
                }

                return points[0];
            }

            // Calculate points on a Bezier curve using the de Casteljau algorithm
            public static List<PointF> BezierCurve(List<PointF> controlPoints, int nPoints)
            {
                List<PointF> points = new List<PointF>();
                double step = 1.0 / (nPoints - 1);

                for (int i = 0; i < nPoints; i++)
                {
                    double t = i * step;
                    points.Add(DeCasteljauPoint(controlPoints, t));
                }

                return points;
            }

            public static List<PointF> IncreaseOrder(List<PointF> controlPoints)
            {
                int n = controlPoints.Count - 1;
                List<PointF> increasedControlPoints = new List<PointF>();

                increasedControlPoints.Add(controlPoints[0]);

                for (int i = 1; i <= n; i++)
                {
                    float x = (i * controlPoints[i - 1].X + (n - i + 1) * controlPoints[i].X) / (n + 1);
                    float y = (i * controlPoints[i - 1].Y + (n - i + 1) * controlPoints[i].Y) / (n + 1);

                    increasedControlPoints.Add(new PointF(x, y));
                }

                increasedControlPoints.Add(controlPoints[n]);

                return increasedControlPoints;
            }


        }

        private void btnIncreaseOrderTop_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = new List<PointF>();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                // Retrieve the values from the DataGridView
                float x = float.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                float y = float.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                // Create a PointF object
                PointF point = new PointF(x, y);
                controlPointsTop.Add(point);
            }

            List<PointF> increasedControlPoints = DeCasteljau.IncreaseOrder(controlPointsTop);

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("xVal", "X");
            dataGridView1.Columns.Add("yVal", "Y");

            for (int i = 0; i < increasedControlPoints.Count; i++)
            {
                dataGridView1.Rows.Add(increasedControlPoints[i].X, increasedControlPoints[i].Y);
            }

            lblOrderTop.Text = "order: " + increasedControlPoints.Count.ToString();

            calculations();
        }

        private void btnIncreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsBottom = new List<PointF>();
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                // Retrieve the values from the DataGridView
                float x = float.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString());
                float y = float.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString());
                // Create a PointF object
                PointF point = new PointF(x, y);
                controlPointsBottom.Add(point);
            }

            List<PointF> increasedControlPoints = DeCasteljau.IncreaseOrder(controlPointsBottom);

            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            dataGridView2.Columns.Add("xVal", "X");
            dataGridView2.Columns.Add("yVal", "Y");

            for (int i = 0; i < increasedControlPoints.Count; i++)
            {
                dataGridView2.Rows.Add(increasedControlPoints[i].X, increasedControlPoints[i].Y);
            }

            lblOrderBottom.Text = "order: " + increasedControlPoints.Count.ToString();

            calculations();
        }

        private void btnDecreaseOrderTop_Click(object sender, EventArgs e)
        {

        }

        private void btnDecreaseOrderBottom_Click(object sender, EventArgs e)
        {

        }
    }
}