using System;
using System.Collections.Generic;
using System.Text;

namespace Hooke_Jeeves
{
    class M1
    {
        List<double> x;
        List<double> x0;
        List<double> xstep;
        List<double> lowerBound;
        List<double> upperBound;
        double eps;
        double coeff;
        int key;

        public M1(List<double> x0, List<double> lowerBound, List<double> upperBound, List<double> xstep, double eps, double coeff, int key)
        {
            this.x0 = new List<double>(x0);

            this.lowerBound = new List<double>(lowerBound);
            this.upperBound = new List<double>(upperBound);

            this.xstep = new List<double>(xstep);

            this.x = new List<double>(x0);

            this.eps = eps;
            this.coeff = coeff;
            this.key = key;
        }

        private int InBounds(List<double> l)
        {
            int key = 0;
            for (int i = 0; i < x.Count; i++)
            {
                if ((l[i] < lowerBound[i]) || (l[i] > upperBound[i]))
                {
                    key = 1;
                }
            }
            return key;
        }

        private double CountFunc(List<double> x)
        {
            double result = 0;
            switch (this.key)
            {
                case 1:
                    result = (double)(100.0 * Math.Pow((Math.Pow(x[0], 2) - x[1]), 2) + Math.Pow(1 - x[0], 2));
                    break;
                case 2:
                    result = (double)(100.0 * Math.Pow((x[1] - Math.Pow(x[0], 3)), 2) + Math.Pow(1 - x[0], 2));
                    break;
                case 3:
                    result = Math.Pow((x[0] + 10 * x[1]), 2) + 5 * Math.Pow((x[2] - x[3]), 2) + Math.Pow((x[1] - 2 * x[2]), 4) + 10 * Math.Pow((x[0] - x[3]), 4);
                    break;
            }
            return result;
        }

        private void InitiateMethod1()
        {
            double fvalue = CountFunc(x0);
            int key = 0;
            int step = 0;
            while (key == 0)
            {
                List<double> tempx = new List<double>(x);
                for (int i = 0; i < x.Count; i++)
                {
                    if (key == 0)
                    {
                        tempx[i] = tempx[i] + xstep[i];
                        List<double> temp1 = new List<double>(tempx);
                        double tempval1 = 0;
                        if (InBounds(temp1) == 0)
                        {
                            tempval1 = CountFunc(temp1);
                        }
                        else
                        {
                            tempval1 = 1000000;
                        }
                        tempx[i] = tempx[i] - 2 * xstep[i];
                        List<double> temp2 = new List<double>(tempx);
                        double tempval2 = 0;
                        if (InBounds(temp2) == 0)
                        {
                            tempval2 = CountFunc(temp2);
                        }
                        else
                        {
                            tempval2 = 1000000;
                        }

                        tempx[i] = tempx[i] + xstep[i];

                        if ((tempval1 < fvalue) || (tempval2 < fvalue))
                        {
                            if (tempval1 < tempval2)
                            {
                                tempx = new List<double>(temp1);
                            }
                            else
                            {
                                tempx = new List<double>(temp2);
                            }
                        }
                        else
                        {
                            xstep[i] = xstep[i] * coeff;
                        }

                        int key1 = 0;
                        for (int j = 0; j < x.Count; j++)
                        {
                            if (xstep[i] > eps)
                            {
                                key1++;
                            }
                        }

                        if (key1 == 0)
                        {
                            key = 1;
                        }
                    }
                }

                step++;
                List<double> tempx0 = new List<double>(tempx);

                for (int j = 0; j < x.Count; j++)
                {
                    tempx0[j] = (double)(x[j] + 2.0 * (tempx[j] - x[j]));
                }

                double tempval3 = 0;

                if (InBounds(tempx0) == 0)
                {
                    tempval3 = CountFunc(tempx0);
                }
                else
                {
                    tempval3 = 1000000;
                }

                if (tempval3 < CountFunc(x))
                {
                    x = new List<double>(tempx0);
                }
                else
                {
                    for (int k = 0; k < x.Count; k++)
                    {
                        xstep[k] = xstep[k] * coeff;
                    }
                }

                fvalue = CountFunc(x);
            }
            Console.WriteLine("Шагов - {0}", step);
        }

        public void Show()
        {
            InitiateMethod1();
            Console.WriteLine("Ответ");
            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine("x{0} = {1}", i, x[i]);
            }
            Console.WriteLine("Q = {0}", CountFunc(x));
        }
    }
}
