using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierAirfoilDesigner
{
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
}
