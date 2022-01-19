using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SolveFx.App
{
    internal class PressF
    {
        private static double x;
        private double result = 0;
        private int n = Environment.ProcessorCount;
        private const int m = 4;
        private Thread thread;

        private double a = 0;
        private double b = 0;
        private static double h = 0;
        private Dictionary<int,double> xRes = new Dictionary<int,double>();

        public PressF(double ax,double bx)
        {
            a = ax;
            b = bx;
            h = (b - a) / (n * m);
            StartingThreads();
        }
        private void StartingThreads()
        {
            for (int i = 0; i < n; i++)
            {
                thread = new Thread(new ParameterizedThreadStart(SolveFx));
                thread.Start(i);
                thread.Join();
            }
            for (int i = 0; i < n; i++)
            {
                result += xRes[i];
            }
            Console.WriteLine(result);
        }

        private void SolveFx(object count)
        {
            double fx = 0;
            x += ((b - a) / n) * (int)count;
            for (int i = 0; i < m; i++)
            {
                x += h;
                fx += h * (Math.Sqrt(1 - (1 / Math.Exp(x))) / (Math.Pow(x, 2) + 1));
                Console.WriteLine("{0}: {1}", Thread.CurrentThread.Name, fx);
                Thread.Sleep(100);
            }
            lock (xRes)
            {
                xRes.Add((int)count, fx);
                Console.WriteLine(Math.Round(xRes[(int)count], 3));
                Console.WriteLine((int)count);
            }
        }
    }
}
