using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    class Nelder_MeadAlg
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
        public delegate double Func(List<double> x);
        Func countFunc;
        enum Bounds { inBounds, outOfBounds }
        enum State { run, stop }
        public Nelder_MeadAlg(List<double> x0, List<double> lowerBound, List<double> upperBound, double eps, double alpha, double beta, double gamma, Func f)
        {
            this.x0 = new List<double>(x0);

            this.lowerBound = new List<double>(lowerBound);
            this.upperBound = new List<double>(upperBound);

            this.x = new List<double>(x0);

            this.eps = eps;
            this.alpha = alpha;
            this.beta = beta;
            this.gamma = gamma;
            this.countFunc = f;
        }
        private int InBounds(List<double> x)
        {
            Bounds key = Bounds.inBounds;
            for (int i = 0; i < x.Count; i++)
            {
                if ((x[i] < lowerBound[i]) || (x[i] > upperBound[i]))
                {
                    key = Bounds.outOfBounds;
                }
            }
            return (int)key;
        }
        private Point CreatePoint(List<double> u, Random r)
        {
            List<double> p = new List<double>(u);
            for (int i = 0; i < x.Count; i++)
            {
                p[i] = p[i] + r.NextDouble();
            }
            Point pnew = new Point(p);
            return pnew;
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

                State key = State.run;

                Dictionary<List<double>, double> grp = new Dictionary<List<double>, double>();
                foreach (var item in p)
                {
                    double temp = countFunc(item.x);
                    grp.Add(item.x, temp);
                }

                grp.Add(x0, countFunc(x0));

                int step = 0;
                while (key == State.run)
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

                    double fr = (InBounds(xr) == 0) ? countFunc(xr) : 1000000;

                    List<double> xe = new List<double>(x0);

                    switch (fr)
                    {
                        case var expression when fr < fl:
                            {
                                for (int i = 0; i < x.Count; i++)
                                {
                                    xe[i] = (1 - beta) * xc[i] + beta * xr[i];
                                }

                                double fe = (InBounds(xe) == 0) ? countFunc(xe) : 1000000;

                                if (fe < fl)
                                {
                                    for (int i = 0; i < grp.Count; i++)
                                    {
                                        if (grp.ElementAt(i).Value == countFunc(xh))
                                        {
                                            grp.Remove(grp.ElementAt(i).Key);
                                        }
                                    }

                                    xh = new List<double>(xe);
                                    grp.Add(xh, countFunc(xh));
                                }
                                else
                                {
                                    if (fr < fe)
                                    {
                                        for (int i = 0; i < grp.Count; i++)
                                        {
                                            if (grp.ElementAt(i).Value == countFunc(xh))
                                            {
                                                grp.Remove(grp.ElementAt(i).Key);
                                            }
                                        }
                                        xh = new List<double>(xr);
                                        grp.Add(xh, countFunc(xh));
                                    }
                                }
                            }
                            break;

                        case var expression when ((fl < fr) && (fr < fg)):
                            {
                                for (int i = 0; i < grp.Count; i++)
                                {
                                    if (grp.ElementAt(i).Value == countFunc(xh))
                                    {
                                        grp.Remove(grp.ElementAt(i).Key);
                                    }
                                }
                                xh = new List<double>(xr);
                                grp.Add(xh, countFunc(xh));
                            }
                            break;
                        case var expression when ((fg < fr) && (fr < fh)):
                            {
                                for (int i = 0; i < grp.Count; i++)
                                {
                                    if (grp.ElementAt(i).Value == countFunc(xh))
                                    {
                                        grp.Remove(grp.ElementAt(i).Key);
                                    }
                                }
                                xh = new List<double>(xr);
                                grp.Add(xh, countFunc(xh));

                                List<double> xs = new List<double>(x0);

                                for (int i = 0; i < x.Count; i++)
                                {
                                    xs[i] = gamma * xh[i] - (1 - gamma) * xc[i];
                                }
                                double fs = countFunc(xs);

                                asc = grp.OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);

                                if (fs < fh)
                                {
                                    for (int i = 0; i < grp.Count; i++)
                                    {
                                        if (grp.ElementAt(i).Value == countFunc(xh))
                                        {
                                            grp.Remove(grp.ElementAt(i).Key);
                                        }
                                    }
                                    xh = new List<double>(xs);
                                    grp.Add(xh, countFunc(xh));
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
                                        grp.Add(xi, countFunc(xi));

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
                                double fs = countFunc(xs);

                                asc = grp.OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);

                                if (fs < fh)
                                {
                                    for (int i = 0; i < grp.Count; i++)
                                    {
                                        if (grp.ElementAt(i).Value == countFunc(xh))
                                        {
                                            grp.Remove(grp.ElementAt(i).Key);
                                        }
                                    }
                                    xh = new List<double>(xs);
                                    grp.Add(xh, countFunc(xh));
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
                                        grp.Add(xi, countFunc(xi));
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
                        double temp0 = Math.Pow(item.Value - countFunc(xl), 2) / x.Count;
                        sum0 += temp0;
                    }
                    double sum = Math.Sqrt(sum0);

                    key = (sum < eps) || (step > 300) ? State.stop : State.run;

                    step++;
                }
            }
            Console.WriteLine("Решение:");
            Console.WriteLine("xlr");

            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine("{0}", xlr[i]);
            }

            Console.WriteLine("Q = {0}", countFunc(xlr));

            Console.WriteLine("xgr");

            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine("{0}", xgr[i]);
            }

            Console.WriteLine("Q = {0}", countFunc(xgr));

            Console.WriteLine("xhr");

            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine("{0}", xhr[i]);
            }

            Console.WriteLine("Q = {0}", countFunc(xhr));
        }
    }
}
