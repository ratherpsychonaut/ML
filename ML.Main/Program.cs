using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            var A = new double[][] { new double[] { 0, 4, 1, 8 }, new double[] { -4, 0, -7, 7 }, new double[] { 1, 0, 3, 4 }, new double[] { 8, 0, 7, 10 } };
            //var A = new double[][] { new double[] { 12, 0, 0, 0 }, new double[] { 1, 0, 0, 22 }, new double[] { 2, -88, 12, 0 }, new double[] { 1, 3, 4, 444 } };
            var X = Matrix.Invert(A); Matrix.Print(X, 3);
            Console.ReadLine();
        }
    }
}
