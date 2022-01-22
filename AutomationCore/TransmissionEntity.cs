using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationCore
{
    public class TransmissionEntity
    {
        public double a { get; set; }
        public double b { get; set; }
        public Point PointA { get; set; }
        public Point PointB { get; set; }
        public double Denominator_horizontal { get; set; }
        public double Denominator_vertical { get; set; }


        public TransmissionEntity(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public TransmissionEntity(double a, double b,double horizontal, double vertical)
        {
            this.a = a;
            this.b = b;
            Denominator_horizontal = horizontal;
            Denominator_vertical = vertical;
        }
        public TransmissionEntity(Point a,Point b)
        {
            PointA = a;
            PointB = b;
        }
    }
}
