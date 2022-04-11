using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nelder_Mead
{
    class Program
    {
        delegate double FuncDel(List<double> x);
        static public double[,] Mult(double[,] A, double[,] x)
        {
            double[,] result = new double[1, A.GetLength(1)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    result[0, i] += A[i, j] * x[0, j];
                }
            }

            return result;
        }
        static public double[,] Add(double[,] A, double[,] x)
        {
            double[,] result = new double[1, A.GetLength(1)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    result[0, j] = A[i, j] + x[0, j];
                }
            }

            return result;
        }
        static public double Func1(List<double> x)
        {
            double result = (double)(100.0 * Math.Pow((Math.Pow(x[0], 2) - x[1]), 2) + Math.Pow(1 - x[0], 2));
            return result;
        }
        static public double Func2(List<double> x)
        {
            double result = (double)(100.0 * Math.Pow((x[1] - Math.Pow(x[0], 3)), 2) + Math.Pow(1 - x[0], 2));
            return result;
        }
        static public double Func3(List<double> x)
        {
            double result = Math.Pow((x[0] + 10 * x[1]), 2) + 5 * Math.Pow((x[2] - x[3]), 2) + Math.Pow((x[1] - 2 * x[2]), 4) + 10 * Math.Pow((x[0] - x[3]), 4);
            return result;
        }
        static public double Func4(List<double> x)
        {
            double[,] newX = new double[1, x.Count];
            for (int i = 0; i < x.Count; i++)
            {
                newX[0, i] = x[i];
            }
            double[,] A = { { 3, 2, 0 }, { 2, -2, 1 }, { 0, 1, -1 } };
            double[,] G = { { 0, 0, 1 } };

            double[,] result = Add(Mult(newX, Mult(A, newX)), Mult(G, newX));

            return result[0, 0];
        }
        static void Main(string[] args)
        {
            List<double> x0 = new List<double>();

            x0.Add(-1.2);
            x0.Add(-1);
            //x0.Add(0);

            //x0.Add(3);
            //x0.Add(-1);
            //x0.Add(0);
            //x0.Add(1);

            List<double> lowerBound = new List<double>();
            List<double> upperBound = new List<double>();

            lowerBound.Add(-1.2);
            lowerBound.Add(-1);
            //lowerBound.Add(-1);
            //lowerBound.Add(-10000);

            upperBound.Add(1);
            upperBound.Add(1);
            //upperBound.Add(1);
            //upperBound.Add(10000);

            double eps = 0.0000001;
            double alpha = 1;
            double beta = 2;
            double gamma = 0.5;

            Nelder_MeadAlg method2 = new Nelder_MeadAlg(x0, lowerBound, upperBound, eps, alpha, beta, gamma, Func2);
            method2.InitiateMethod2();
        }
    }
}
