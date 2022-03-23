using System;
using System.Collections.Generic;

namespace Nelder_Mead
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

            int key = 1;
            double eps = 0.0000001;
            double alpha = 1;
            double beta = 2;
            double gamma = 0.5;

            M2 method2 = new M2(x0, lowerBound, upperBound, eps, alpha, beta, gamma, key);
            method2.InitiateMethod2();
        }
    }
}
