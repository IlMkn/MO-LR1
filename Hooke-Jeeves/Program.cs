using System;
using System.Collections.Generic;

namespace Hooke_Jeeves
{
    class Program
    {
        static void Main(string[] args)
        {
            List<double> x0 = new List<double>();
            x0.Add(-1.2);
            x0.Add(-1);
            //x0.Add(3);
            //x0.Add(-1);
            //x0.Add(0);
            //x0.Add(1);

            List<double> lowerBound = new List<double>();
            List<double> upperBound = new List<double>();

            lowerBound.Add(-1.2);
            lowerBound.Add(-1);
            upperBound.Add(1);
            upperBound.Add(1);

            List<double> xstep = new List<double>();
            xstep.Add(1);
            xstep.Add(1);

            //xstep.Add(1);
            //xstep.Add(1);
            //xstep.Add(1);
            //xstep.Add(1);

            int key = 1;
            double eps = 0.0000001;
            double coeff = 0.4;

            M1 method = new M1(x0, lowerBound, upperBound, xstep, eps, coeff, key);
            method.Show();
        }
    }
}
