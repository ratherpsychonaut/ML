using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Main
{
    static class GradientDescent
    {
        //GradientDescent.SingleVariable(x => x * 2, 10, 0.01, 0.00001, 10000);
        //GradientDescent.SingleVariable(x => (4 * x - 9) * x * x, 6, 0.01, 0.00001, 10000) //=2.2499646074278457 after 70 iterations
        public static double SingleVariable(Func<double, double> derivative, double x0, double learningRate, double precision, int maxsteps)
        {
            double step; var x = x0; int i = 1;
            do
            {
                step = learningRate * derivative(x);
                Console.WriteLine(string.Format("Step {0}: {1} minus {2} = {3}", i, x, step, x - step));
                x -= step; i++;
            }
            while ((Math.Abs(step) > precision) && (i <= maxsteps));
            Console.WriteLine("RESULT: {0} after {1} iterations", x, i - 1);
            return x;
        }

        public static double[] LinearGD(double[][] A, double[][] B, double learningRate, double precision, int maxsteps)
        {
            var n = A.Length; var m = A[0].Length;
            var derivative = new double[n][]; for (int k = 0; k < n; k++) derivative[k] = new double[m];
            var X = new double[m]; for (int k = 0; k < m; k++) X[k] = 1;
            var step = new double[m]; int i = 1;
            do
            {
                //2/N * AT * (A*X - B)
                derivative = Matrix.Scale(Matrix.Multiply(Matrix.Scale(Matrix.Transpose(A), 2), Matrix.Substract(Matrix.Multiply(A, Matrix.ArrayToVector(X)), B)), 2.0 / n);
                step = Matrix.VectorToArray(Matrix.Scale(derivative, learningRate));
                i++;
                for (int k = 0; k < X.Length; k++) X[k] -= step[k];
            }
            while ((Matrix.VectorModule(step) > precision) && (i <= maxsteps));
            Console.WriteLine("Steps: " + i);
            return X;
        }

        public static void Test()
        {
            //https://towardsdatascience.com/linear-regression-simplified-ordinary-least-square-vs-gradient-descent-48145de2cf76
            var A = Matrix.ArrayToVector(new double[] { 2, 3, 5, 13, 8, 16, 11, 1, 9 });
            var B = Matrix.ArrayToVector(new double[] { 15, 28, 42, 64, 50, 90, 58, 8, 54 });

            //Matrix.Print(LinearCF(A, B));

            //var I = Input.ReadCSV("..\\..\\Data1.csv", "\n");
            //var A = Matrix.GetCol(I, 0);
            //var B = Matrix.GetCol(I, 1);

            //var I = Input.ReadCSV11("..\\..\\Advertising.csv");
            //var A = Matrix.GetCols(I, new int[] { 0, 1, 2 });
            //var B = Matrix.GetCol(I, 3);

            var A1 = Matrix.AddCol1(A);
            var R = LinearGD(A1, B, 0.00001, 0.0000001, 90000); Matrix.PrintVector(R, 4);
        }
    }
}
