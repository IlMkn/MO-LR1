using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hooke_Jeeves
{
    class Hooke_JeevesAlg
    {
        List<double> x;
        List<double> x0;
        List<double> xstep;
        List<double> lowerBound;
        List<double> upperBound;
        double eps;
        double coeff;
        public delegate double Func(List<double> x);
        Func countFunc;
        enum Bounds { inBounds, outOfBounds }
        enum State { run, stop }

        public Hooke_JeevesAlg(List<double> x0, List<double> lowerBound, List<double> upperBound, List<double> xstep, double eps, double coeff, Func f)
        {
            this.x0 = new List<double>(x0);

            this.lowerBound = new List<double>(lowerBound);
            this.upperBound = new List<double>(upperBound);

            this.xstep = new List<double>(xstep);

            this.x = new List<double>(x0);

            this.eps = eps;
            this.coeff = coeff;
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
        private void InitiateMethod1()
        {
            double fvalue = countFunc(x0);
            State key = State.run;
            int step = 0;
            while (key == State.run)
            {
                List<double> tempx = new List<double>(x);
                for (int i = 0; i < x.Count; i++)
                {
                    if (key == State.run)
                    {
                        tempx[i] = tempx[i] + xstep[i];
                        List<double> temp1 = new List<double>(tempx);

                        double tempval1 = (InBounds(temp1) == 0) ? countFunc(temp1) : 1000000;

                        tempx[i] = tempx[i] - 2 * xstep[i];
                        List<double> temp2 = new List<double>(tempx);

                        double tempval2 = (InBounds(temp2) == 0) ? countFunc(temp2) : 1000000;

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
                        State key1 = State.stop;
                        for (int j = 0; j < x.Count; j++)
                        {
                            if (xstep[i] > eps)
                            {
                                key1 = State.run;
                            }
                        }

                        if (key1 == State.stop)
                        {
                            key = State.stop;
                        }
                    }
                }

                step++;
                List<double> tempx0 = new List<double>(tempx);

                for (int j = 0; j < x.Count; j++)
                {
                    tempx0[j] = (double)(x[j] + 2.0 * (tempx[j] - x[j]));
                }

                double tempval3 = (InBounds(tempx0) == 0) ? countFunc(tempx0) : 1000000;

                if (tempval3 < countFunc(x))
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

                fvalue = countFunc(x);
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
            Console.WriteLine("Q = {0}", countFunc(x));
        }
    }
}
