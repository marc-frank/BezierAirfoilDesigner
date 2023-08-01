using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierAirfoilDesigner
{
    public class CircleProperties
    {
        public static PointD CalculateMidpoint(PointD p1, PointD p2, PointD p3)
        {
            double D = 2 * (p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y));

            double Ux = ((p1.X * p1.X + p1.Y * p1.Y) * (p2.Y - p3.Y) + (p2.X * p2.X + p2.Y * p2.Y) * (p3.Y - p1.Y) + (p3.X * p3.X + p3.Y * p3.Y) * (p1.Y - p2.Y)) / D;
            double Uy = ((p1.X * p1.X + p1.Y * p1.Y) * (p3.X - p2.X) + (p2.X * p2.X + p2.Y * p2.Y) * (p1.X - p3.X) + (p3.X * p3.X + p3.Y * p3.Y) * (p2.X - p1.X)) / D;

            return new PointD(Ux, Uy);
        }

        public static double CalculateRadius(PointD p1, PointD p2, PointD p3)
        {
            PointD midpoint = CalculateMidpoint(p1, p2, p3);

            double radius = (double)Math.Sqrt(Math.Pow(p1.X - midpoint.X, 2) + Math.Pow(p1.Y - midpoint.Y, 2));

            return radius;
        }
    }
}
