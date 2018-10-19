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

            var X = Input.ReadCSV("..\\..\\Advertising.csv");

            var B1 = Regression.LinearCF(Matrix.GetCol(X, 0), Matrix.GetCol(X, 3));
            var B2 = Regression.LinearCF(Matrix.GetCol(X, 1), Matrix.GetCol(X, 3));
            var B3 = Regression.LinearCF(Matrix.GetCol(X, 2), Matrix.GetCol(X, 3));
            var B = Regression.LinearCF(Matrix.GetCols(X, new int[] { 0, 1, 2 }), Matrix.GetCol(X, 3));

            Matrix.Print(B1); //TV -> b0 = 7.03, b1 = 0.0475
            Matrix.Print(B2); //radio -> b0 = 9.312, b1 = 0.203
            Matrix.Print(B3); //newsp -> b0 = 12.351, b1 = 0.055
            Matrix.Print(B); //all 3 -> b0 = 2.939, TV = 0.046, radio = 0.189, np = -0.001

            Console.ReadLine();
        }
    }
}
