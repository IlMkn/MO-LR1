using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nelder_Mead
{
    class Point
    {
        public List<double> x { get; set; }

        public Point(List<double> x)
        {
            this.x = new List<double>(x);
        }

        public void Show()
        {
            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine(x[i]);
            }
        }

    }
    class M2
    {
        double eps;
        double alpha;
        double beta;
        double gamma;
        List<double> lowerBound;
        List<double> upperBound;
        List<double> x;
        List<double> x0;
        List<Point> p;
        int key;

        public M2(List<double> x0, List<double> lowerBound, List<double> upperBound, double eps, double alpha, double beta, double gamma, int key)
        {
            this.x0 = new List<double>(x0);

            this.lowerBound = new List<double>(lowerBound);
            this.upperBound = new List<double>(upperBound);

            this.x = new List<double>(x0);

            this.eps = eps;
            this.alpha = alpha;
            this.beta = beta;
            this.gamma = gamma;
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

        private Point CreatePoint(List<double> u, Random r)
        {
            List<double> p = new List<double>(u);
            for (int i = 0; i < x.Count; i++)
            {
                //p[i] = p[i] + r.NextDouble()*100;
                p[i] = p[i] + r.NextDouble();
            }
            Point pnew = new Point(p);
            return pnew;
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

        public void InitiateMethod2()
        {

            List<double> xlr = new List<double>(x0);
            List<double> xgr = new List<double>(x0);
            List<double> xhr = new List<double>(x0);

            double flr = 10000;
            double fgr = 10000;
            double fhr = 10000;


            Random r = new Random();

            for (int f = 0; f < 5; f++)
            {
                p = new List<Point>();
                for (int i = 0; i < x.Count; i++)
                {
                    p.Add(CreatePoint(x0, r));
                }
                int key = 0;


                Dictionary<List<double>, double> grp = new Dictionary<List<double>, double>();
                foreach (var item in p)
                {
                    double temp = CountFunc(item.x);
                    grp.Add(item.x, temp);
                }

                grp.Add(x0, CountFunc(x0));

                int step = 0;
                while (key == 0)
                {
                    var asc = grp.OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);

                    double fl = asc.ElementAt(0).Value;
                    double fg = asc.ElementAt(x.Count - 1).Value;
                    double fh = asc.ElementAt(x.Count).Value;

                    List<double> xl = new List<double>(asc.ElementAt(0).Key);
                    List<double> xg = new List<double>(asc.ElementAt(x.Count - 1).Key);
                    List<double> xh = new List<double>(asc.ElementAt(x.Count).Key);

                    List<double> xc = new List<double>(x0);
                    for (int i = 0; i < x.Count; i++)
                    {
                        xc[i] = 0;
                    }

                    for (int i = 0; i < x.Count; i++)
                    {
                        for (int j = 0; j < x.Count; j++)
                        {
                            xc[i] += asc.ElementAt(j).Key[i];
                        }
                    }

                    for (int i = 0; i < x.Count; i++)
                    {
                        xc[i] = xc[i] / x.Count;
                    }

                    List<double> xr = new List<double>(x0);
                    for (int i = 0; i < x.Count; i++)
                    {
                        xr[i] = (1 + alpha) * xc[i] - alpha * xh[i];
                    }

                    double fr = 0;

                    if (InBounds(xr) == 0)
                    {
                        fr = CountFunc(xr);
                    }
                    else
                    {
                        fr = 1000000;
                    }

                    List<double> xe = new List<double>(x0);

                    switch (fr)
                    {
                        case var expression when fr < fl:
                            {
                                for (int i = 0; i < x.Count; i++)
                                {
                                    xe[i] = (1 - beta) * xc[i] + beta * xr[i];
                                }
                                double fe = 0;

                                if (InBounds(xe) == 0)
                                {
                                    fe = CountFunc(xe);
                                }
                                else
                                {
                                    fe = 1000000;
                                }


                                if (fe < fl)
                                {
                                    for (int i = 0; i < grp.Count; i++)
                                    {
                                        if (grp.ElementAt(i).Value == CountFunc(xh))
                                        {
                                            grp.Remove(grp.ElementAt(i).Key);
                                        }
                                    }

                                    xh = new List<double>(xe);
                                    grp.Add(xh, CountFunc(xh));
                                }
                                else
                                {
                                    if (fr < fe)
                                    {
                                        for (int i = 0; i < grp.Count; i++)
                                        {
                                            if (grp.ElementAt(i).Value == CountFunc(xh))
                                            {
                                                grp.Remove(grp.ElementAt(i).Key);
                                            }
                                        }
                                        xh = new List<double>(xr);
                                        grp.Add(xh, CountFunc(xh));
                                    }
                                }
                            }
                            break;

                        case var expression when ((fl < fr) && (fr < fg)):
                            {
                                for (int i = 0; i < grp.Count; i++)
                                {
                                    if (grp.ElementAt(i).Value == CountFunc(xh))
                                    {
                                        grp.Remove(grp.ElementAt(i).Key);
                                    }
                                }
                                xh = new List<double>(xr);
                                grp.Add(xh, CountFunc(xh));
                            }
                            break;
                        case var expression when ((fg < fr) && (fr < fh)):
                            {
                                for (int i = 0; i < grp.Count; i++)
                                {
                                    if (grp.ElementAt(i).Value == CountFunc(xh))
                                    {
                                        grp.Remove(grp.ElementAt(i).Key);
                                    }
                                }
                                xh = new List<double>(xr);
                                grp.Add(xh, CountFunc(xh));

                                List<double> xs = new List<double>(x0);

                                for (int i = 0; i < x.Count; i++)
                                {
                                    xs[i] = gamma * xh[i] - (1 - gamma) * xc[i];
                                }
                                double fs = CountFunc(xs);

                                asc = grp.OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);

                                if (fs < fh)
                                {
                                    for (int i = 0; i < grp.Count; i++)
                                    {
                                        if (grp.ElementAt(i).Value == CountFunc(xh))
                                        {
                                            grp.Remove(grp.ElementAt(i).Key);
                                        }
                                    }
                                    xh = new List<double>(xs);
                                    grp.Add(xh, CountFunc(xh));
                                }
                                else
                                {
                                    for (int j = 1; j < x.Count + 1; j++)
                                    {

                                        List<double> temp = new List<double>(asc.ElementAt(j).Key);
                                        List<double> temp1 = asc.ElementAt(j).Key;
                                        List<double> xi = new List<double>(x0);

                                        for (int i = 0; i < x.Count; i++)
                                        {
                                            xi[i] = xl[i] + (temp[i] - xl[i]) / 2;
                                        }
                                        grp.Remove(temp1);
                                        grp.Add(xi, CountFunc(xi));

                                    }
                                }
                            }
                            break;
                        case var expression when (fh < fr):
                            {
                                List<double> xs = new List<double>(x0);

                                for (int i = 0; i < x.Count; i++)
                                {
                                    xs[i] = gamma * xh[i] - (1 - gamma) * xc[i];
                                }
                                double fs = CountFunc(xs);

                                asc = grp.OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);

                                if (fs < fh)
                                {
                                    for (int i = 0; i < grp.Count; i++)
                                    {
                                        if (grp.ElementAt(i).Value == CountFunc(xh))
                                        {
                                            grp.Remove(grp.ElementAt(i).Key);
                                        }
                                    }
                                    xh = new List<double>(xs);
                                    grp.Add(xh, CountFunc(xh));
                                }
                                else
                                {
                                    for (int j = 1; j < x.Count + 1; j++)
                                    {

                                        List<double> temp = new List<double>(asc.ElementAt(j).Key);
                                        List<double> temp1 = asc.ElementAt(j).Key;
                                        List<double> xi = new List<double>(x0);

                                        for (int i = 0; i < x.Count; i++)
                                        {
                                            xi[i] = xl[i] + (temp[i] - xl[i]) / 2;
                                        }
                                        grp.Remove(temp1);
                                        grp.Add(xi, CountFunc(xi));
                                    }
                                }
                            }
                            break;
                    }

                    double sum0 = 0;
                    var asc1 = new Dictionary<List<double>, double>(grp.OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value));
                    xl = new List<double>(asc1.ElementAt(0).Key);
                    xg = new List<double>(asc1.ElementAt(x.Count - 1).Key);
                    xh = new List<double>(asc1.ElementAt(x.Count).Key);

                    if (fl < flr)
                    {
                        xlr = new List<double>(xl);
                        flr = fl;

                        xgr = new List<double>(xg);
                        fgr = fg;

                        xhr = new List<double>(xh);
                        fhr = fh;
                    }
                    foreach (var item in asc1)
                    {
                        double temp0 = Math.Pow(item.Value - CountFunc(xl), 2) / x.Count;
                        sum0 += temp0;
                    }
                    double sum = Math.Sqrt(sum0);

                    if ((sum < eps) || (step > 300))
                    {
                        key = 1;
                    }
                    step++;
                }

            }
            Console.WriteLine("Решение:");
            Console.WriteLine("xlr");
            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine("{0}", xlr[i]);
            }

            Console.WriteLine("Q = {0}", CountFunc(xlr));

            Console.WriteLine("xgr");
            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine("{0}", xgr[i]);
            }

            Console.WriteLine("Q = {0}", CountFunc(xgr));

            Console.WriteLine("xhr");
            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine("{0}", xhr[i]);
            }

            Console.WriteLine("Q = {0}", CountFunc(xhr));
        }
    }
}
