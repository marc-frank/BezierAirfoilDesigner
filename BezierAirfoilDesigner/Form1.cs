using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Windows.Forms;
using System.Numerics;

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
            formsPlot1.Plot.AxisScaleLock(enable: true, scaleMode: ScottPlot.EqualScaleMode.PreserveX);

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

            var midline = formsPlot1.Plot.AddScatterList(color: Color.Gray, lineStyle: ScottPlot.LineStyle.Dash, markerSize: 0);

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

                midline.Add(pointsTop[i].X, mid);
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

        public class CircleProperties
        {
            public static PointF CalculateMidpoint(PointF p1, PointF p2, PointF p3)
            {
                float D = 2 * (p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y));

                float Ux = ((p1.X * p1.X + p1.Y * p1.Y) * (p2.Y - p3.Y) + (p2.X * p2.X + p2.Y * p2.Y) * (p3.Y - p1.Y) + (p3.X * p3.X + p3.Y * p3.Y) * (p1.Y - p2.Y)) / D;
                float Uy = ((p1.X * p1.X + p1.Y * p1.Y) * (p3.X - p2.X) + (p2.X * p2.X + p2.Y * p2.Y) * (p1.X - p3.X) + (p3.X * p3.X + p3.Y * p3.Y) * (p2.X - p1.X)) / D;

                return new PointF(Ux, Uy);
            }

            public static float CalculateRadius(PointF p1, PointF p2, PointF p3)
            {
                PointF midpoint = CalculateMidpoint(p1, p2, p3);

                float radius = (float)Math.Sqrt(Math.Pow(p1.X - midpoint.X, 2) + Math.Pow(p1.Y - midpoint.Y, 2));

                return radius;
            }
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

            public static List<PointF> DecreaseOrder(List<PointF> controlPoints)
            {
                int n = controlPoints.Count - 1;
                List<PointF> decreasedControlPoints = new List<PointF>();

                decreasedControlPoints.Add(controlPoints[0]);

                for (int i = 1; i < n; i++)
                {
                    float x = ((n + 1) * controlPoints[i].X - i * decreasedControlPoints[i - 1].X) / (n - i + 1);
                    float y = ((n + 1) * controlPoints[i].Y - i * decreasedControlPoints[i - 1].Y) / (n - i + 1);

                    decreasedControlPoints.Add(new PointF(x, y));
                }

                decreasedControlPoints[decreasedControlPoints.Count - 1] = controlPoints[controlPoints.Count - 1];

                return decreasedControlPoints;
            }


            //-----------------------------------------------------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------------------------------------------------------------------

            public static List<PointF> DecreaseOrder2(List<PointF> controlPoints)
            {
                int n = controlPoints.Count - 1;
                if (n <= 0)
                    throw new System.Exception("No further dimensional reduction possible!");

                // Construct the Matrix
                float[,] M = new float[n + 1, n];
                M[0, 0] = 1;
                M[n, n - 1] = 1;
                for (int i = 1; i < n; i++)
                {
                    M[i, i - 1] = i / (float)(n + 1);
                    M[i, i] = 1 - i / (float)(n + 1);
                }

                // Find the least squares solution
                float[,] K = Multiply(Inverse(Multiply(Transpose(M), M)), Transpose(M));

                PointF[] pts = controlPoints.ToArray();
                PointF[] result = new PointF[K.GetLength(0)];
                for (int i = 0; i < K.GetLength(0); i++)
                {
                    float x = 0, y = 0;
                    for (int j = 0; j < K.GetLength(1); j++)
                    {
                        x += K[i, j] * pts[j].X;
                        y += K[i, j] * pts[j].Y;
                    }
                    result[i] = new PointF(x, y);
                }

                result[0] = controlPoints[0];
                result[n - 1] = controlPoints[n];

                return new List<PointF>(result);
            }

            private static float[,] Transpose(float[,] matrix)
            {
                int rows = matrix.GetLength(0);
                int columns = matrix.GetLength(1);
                float[,] result = new float[columns, rows];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        result[j, i] = matrix[i, j];
                    }
                }
                return result;
            }

            private static float[,] Multiply(float[,] matrix1, float[,] matrix2)
            {
                int rows1 = matrix1.GetLength(0);
                int columns1 = matrix1.GetLength(1);
                int rows2 = matrix2.GetLength(0);
                int columns2 = matrix2.GetLength(1);

                if (columns1 != rows2)
                    throw new ArgumentException("Matrix dimensions are not compatible for multiplication.");

                float[,] result = new float[rows1, columns2];

                for (int i = 0; i < rows1; i++)
                {
                    for (int j = 0; j < columns2; j++)
                    {
                        float sum = 0;
                        for (int k = 0; k < columns1; k++)
                        {
                            sum += matrix1[i, k] * matrix2[k, j];
                        }
                        result[i, j] = sum;
                    }
                }

                return result;
            }


            private static float[,] Inverse(float[,] matrix)
            {
                int size = matrix.GetLength(0);
                float[,] augmentedMatrix = new float[size, 2 * size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        augmentedMatrix[i, j] = matrix[i, j];
                    }
                    augmentedMatrix[i, size + i] = 1;
                }

                for (int i = 0; i < size; i++)
                {
                    float pivot = augmentedMatrix[i, i];
                    for (int j = i; j < 2 * size; j++)
                    {
                        augmentedMatrix[i, j] /= pivot;
                    }
                    for (int j = 0; j < size; j++)
                    {
                        if (j != i)
                        {
                            float factor = augmentedMatrix[j, i];
                            for (int k = i; k < 2 * size; k++)
                            {
                                augmentedMatrix[j, k] -= factor * augmentedMatrix[i, k];
                            }
                        }
                    }
                }

                float[,] inverse = new float[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        inverse[i, j] = augmentedMatrix[i, size + j];
                    }
                }

                return inverse;
            }

            //-----------------------------------------------------------------------------------------------------------------------------------

            public static double CalculateRadius(List<PointF> controlPoints)
            {
                if (controlPoints.Count < 2)
                {
                    throw new ArgumentException("Expected at least 2 control points for a Bezier curve.");
                }

                var P0 = new Vector2(controlPoints[0].X, controlPoints[0].Y);
                var P1 = new Vector2(controlPoints[1].X, controlPoints[1].Y);

                var B1 = P1 - P0; // First derivative at t=0

                // If there is only two points, it's a straight line and radius of curvature is infinity
                if (controlPoints.Count == 2)
                {
                    return double.PositiveInfinity;
                }

                var P2 = new Vector2(controlPoints[2].X, controlPoints[2].Y);
                var B2 = P2 - 2 * P1 + P0; // Second derivative at t=0

                var numerator = Math.Abs(B1.X * B2.Y - B1.Y * B2.X);
                var denominator = Math.Pow(Math.Pow(B1.X, 2) + Math.Pow(B1.Y, 2), 1.5);

                // Check for division by zero, which would suggest a straight line
                if (denominator == 0)
                {
                    return double.PositiveInfinity; // radius of curvature is infinity for a straight line
                }

                var curvature = numerator / denominator;
                var radius = 1 / curvature;

                return radius;
            }
        }

        private void btnIncreaseOrderTop_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);
            controlPointsTop = DeCasteljau.IncreaseOrder(controlPointsTop);

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("xVal", "X");
            dataGridView1.Columns.Add("yVal", "Y");

            for (int i = 0; i < controlPointsTop.Count; i++)
            {
                dataGridView1.Rows.Add(controlPointsTop[i].X, controlPointsTop[i].Y);
            }

            calculations();
        }

        private void btnIncreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);

            controlPointsBottom = DeCasteljau.IncreaseOrder(controlPointsBottom);

            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            dataGridView2.Columns.Add("xVal", "X");
            dataGridView2.Columns.Add("yVal", "Y");

            for (int i = 0; i < controlPointsBottom.Count; i++)
            {
                dataGridView2.Rows.Add(controlPointsBottom[i].X, controlPointsBottom[i].Y);
            }

            calculations();
        }

        private void btnDecreaseOrderTop_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsTop = GetControlPoints(dataGridView1);

            if (controlPointsTop.Count <= 2) { return; }

            controlPointsTop = DeCasteljau.DecreaseOrder(controlPointsTop);

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("xVal", "X");
            dataGridView1.Columns.Add("yVal", "Y");

            for (int i = 0; i < controlPointsTop.Count; i++)
            {
                dataGridView1.Rows.Add(controlPointsTop[i].X, controlPointsTop[i].Y);
            }

            calculations();
        }

        private void btnDecreaseOrderBottom_Click(object sender, EventArgs e)
        {
            List<PointF> controlPointsBottom = GetControlPoints(dataGridView2);

            if (controlPointsBottom.Count <= 2) { return; }

            controlPointsBottom = DeCasteljau.DecreaseOrder(controlPointsBottom);

            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            dataGridView2.Columns.Add("xVal", "X");
            dataGridView2.Columns.Add("yVal", "Y");

            for (int i = 0; i < controlPointsBottom.Count; i++)
            {
                dataGridView2.Rows.Add(controlPointsBottom[i].X, controlPointsBottom[i].Y);
            }

            calculations();
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