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

        public Airfoil()
        {
            airfoilName = "Default Airfoil";
            numberOfCurvePoints = 150;
            
            controlPointsTop = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(0, 0.1),
                new PointD(0.5, 0.1),
                new PointD(1, 0)
            };
    
            controlPointsBottom = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(0, -0.1),
                new PointD(0.5, -0.1),
                new PointD(1, 0)
            };

            curvePointsTop = new List<PointD>();
            curvePointsBottom = new List<PointD>();

            CalculateCurvePoints();
        }

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
            set
            {
                if (value >= 2)
                {
                    numberOfCurvePoints = value;
                    CalculateCurvePoints();
                }
                else
                {
                    MessageBox.Show("Invalid number of curve points. Value must be at least 2.");
                }
            }
        }

        public List<PointD> ControlPointsTop
        {
            get { return controlPointsTop; }
            set { controlPointsTop = value; CalculateCurvePoints(); }
        }

        public List<PointD> ControlPointsBottom
        {
            get { return controlPointsBottom; }
            set { controlPointsBottom = value; CalculateCurvePoints(); }
        }

        public List<PointD> CurvePointsTop
        {
            get { return curvePointsTop; }
            set { curvePointsTop = value; }
        }

        public List<PointD> CurvePointsBottom
        {
            get { return curvePointsBottom; }
            set { curvePointsBottom = value; }
        }

        public void CalculateCurvePoints()
        {
            curvePointsTop = DeCasteljau.BezierCurve(controlPointsTop, numberOfCurvePoints);
            curvePointsBottom = DeCasteljau.BezierCurve(controlPointsBottom, numberOfCurvePoints);
        }
    }
}
