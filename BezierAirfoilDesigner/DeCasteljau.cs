using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Numerics;

namespace BezierAirfoilDesigner
{
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
            if (controlPoints.Count <= 2) { return controlPoints; }
         
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
}

