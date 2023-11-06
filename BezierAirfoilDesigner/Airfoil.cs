using System;
using System.Collections.Generic;

namespace BezierAirfoilDesigner
{
    internal class Airfoil
    {
        private string airfoilName;
        private int numberOfCurvePoints;
        private List<PointD> controlPointsTop;
        private List<PointD> controlPointsBottom;
        private List<PointD> curvePointsTop;
        private List<PointD> curvePointsBottom;

        public Airfoil(string name, List<PointD> controlPointsTop, List<PointD> controlPointsBottom)
        {
            airfoilName = name;
            this.controlPointsTop = controlPointsTop;
            this.controlPointsBottom = controlPointsBottom;
            CalculateCurvePoints();
        }

        public string AirfoilName
        {
            get { return airfoilName; }
            set { airfoilName = value; }
        }

        public int NumberOfCurvePoints
        {
            get { return numberOfCurvePoints; }
            set { numberOfCurvePoints = value; }
        }

        public List<PointD> ControlPointsTop
        {
            get { return controlPointsTop; }
            set { controlPointsTop = value; }
        }

        public List<PointD> ControlPointsBottom
        {
            get { return controlPointsBottom; }
            set { controlPointsBottom = value; }
        }

        public List<PointD> CurvePointsTop
        {
            get { return curvePointsTop; }
        }

        public List<PointD> CurvePointsBottom
        {
            get { return curvePointsBottom; }
        }

        private void CalculateCurvePoints()
        {
            curvePointsTop = DeCasteljau.BezierCurve(controlPointsTop, numberOfCurvePoints);
            curvePointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numberOfCurvePoints);
        }
    }
}
