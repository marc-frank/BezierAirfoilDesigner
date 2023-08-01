using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Numerics;
using MathNet.Numerics.Optimization;

namespace BezierAirfoilDesigner
{
    public class DeCasteljau
    {
        // Calculate a point on a Bezier curve using the de Casteljau algorithm
        private static PointD DeCasteljauPoint(List<PointD> controlPoints, double t)
        {
            List<PointD> points = new List<PointD>(controlPoints);
            int n = points.Count;

            for (int r = 1; r < n; r++)
            {
                for (int i = 0; i < n - r; i++)
                {
                    double x = (1 - (double)t) * points[i].X + (double)t * points[i + 1].X;
                    double y = (1 - (double)t) * points[i].Y + (double)t * points[i + 1].Y;
                    points[i] = new PointD(x, y);
                }
            }

            return points[0];
        }

        // Calculate points on a Bezier curve using the de Casteljau algorithm
        public static List<PointD> BezierCurve(List<PointD> controlPoints, int nPoints)
        {
            List<PointD> points = new List<PointD>();
            double step = 1.0 / (nPoints - 1);

            for (int i = 0; i < nPoints; i++)
            {
                double t = i * step;
                points.Add(DeCasteljauPoint(controlPoints, t));
            }

            return points;
        }

        public static List<PointD> IncreaseOrder(List<PointD> controlPoints)
        {
            int n = controlPoints.Count - 1;
            List<PointD> increasedControlPoints = new List<PointD>();

            increasedControlPoints.Add(controlPoints[0]);

            for (int i = 1; i <= n; i++)
            {
                double x = (i * controlPoints[i - 1].X + (n - i + 1) * controlPoints[i].X) / (n + 1);
                double y = (i * controlPoints[i - 1].Y + (n - i + 1) * controlPoints[i].Y) / (n + 1);

                increasedControlPoints.Add(new PointD(x, y));
            }

            increasedControlPoints.Add(controlPoints[n]);

            return increasedControlPoints;
        }

        public static List<PointD> DecreaseOrder(List<PointD> controlPoints)
        {
            if (controlPoints.Count <= 2) { return controlPoints; }
         
            int n = controlPoints.Count - 1;
            List<PointD> decreasedControlPoints = new List<PointD>();

            decreasedControlPoints.Add(controlPoints[0]);

            for (int i = 1; i < n; i++)
            {
                double x = ((n + 1) * controlPoints[i].X - i * decreasedControlPoints[i - 1].X) / (n - i + 1);
                double y = ((n + 1) * controlPoints[i].Y - i * decreasedControlPoints[i - 1].Y) / (n - i + 1);

                decreasedControlPoints.Add(new PointD(x, y));
            }

            decreasedControlPoints[decreasedControlPoints.Count - 1] = controlPoints[controlPoints.Count - 1];

            return decreasedControlPoints;
        }

        public static List<PointD> DecreaseOrder2(List<PointD> controlPoints)
        {
            int n = controlPoints.Count - 1;
            if (n <= 0)
                throw new System.Exception("No further dimensional reduction possible!");

            // Construct the Matrix
            double[,] M = new double[n + 1, n];
            M[0, 0] = 1;
            M[n, n - 1] = 1;
            for (int i = 1; i < n; i++)
            {
                M[i, i - 1] = i / (double)(n + 1);
                M[i, i] = 1 - i / (double)(n + 1);
            }

            // Find the least squares solution
            double[,] K = Multiply(Inverse(Multiply(Transpose(M), M)), Transpose(M));

            PointD[] pts = controlPoints.ToArray();
            PointD[] result = new PointD[K.GetLength(0)];
            for (int i = 0; i < K.GetLength(0); i++)
            {
                double x = 0, y = 0;
                for (int j = 0; j < K.GetLength(1); j++)
                {
                    x += K[i, j] * pts[j].X;
                    y += K[i, j] * pts[j].Y;
                }
                result[i] = new PointD(x, y);
            }

            result[0] = controlPoints[0];
            result[n - 1] = controlPoints[n];

            return new List<PointD>(result);
        }

        private static double[,] Transpose(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);
            double[,] result = new double[columns, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }
            return result;
        }

        private static double[,] Multiply(double[,] matrix1, double[,] matrix2)
        {
            int rows1 = matrix1.GetLength(0);
            int columns1 = matrix1.GetLength(1);
            int rows2 = matrix2.GetLength(0);
            int columns2 = matrix2.GetLength(1);

            if (columns1 != rows2)
                throw new ArgumentException("Matrix dimensions are not compatible for multiplication.");

            double[,] result = new double[rows1, columns2];

            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < columns2; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < columns1; k++)
                    {
                        sum += matrix1[i, k] * matrix2[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        private static double[,] Inverse(double[,] matrix)
        {
            int size = matrix.GetLength(0);
            double[,] augmentedMatrix = new double[size, 2 * size];
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
                double pivot = augmentedMatrix[i, i];
                for (int j = i; j < 2 * size; j++)
                {
                    augmentedMatrix[i, j] /= pivot;
                }
                for (int j = 0; j < size; j++)
                {
                    if (j != i)
                    {
                        double factor = augmentedMatrix[j, i];
                        for (int k = i; k < 2 * size; k++)
                        {
                            augmentedMatrix[j, k] -= factor * augmentedMatrix[i, k];
                        }
                    }
                }
            }

            double[,] inverse = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    inverse[i, j] = augmentedMatrix[i, size + j];
                }
            }

            return inverse;
        }

        public static double CalculateRadius(List<PointD> controlPoints)
        {
            if (controlPoints.Count < 2)
            {
                throw new ArgumentException("Expected at least 2 control points for a Bezier curve.");
            }

            var P0 = new Vector2((float)controlPoints[0].X, (float)controlPoints[0].Y);
            var P1 = new Vector2((float)controlPoints[1].X, (float)controlPoints[1].Y);

            var B1 = P1 - P0; // First derivative at t=0

            // If there is only two points, it's a straight line and radius of curvature is infinity
            if (controlPoints.Count == 2)
            {
                return double.PositiveInfinity;
            }

            var P2 = new Vector2((float)controlPoints[2].X, (float)controlPoints[2].Y);
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

