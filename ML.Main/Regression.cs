using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Main
{
    static class Regression
    {
        /* Linear Regression
         * 
         * given n observations of k input variables [x1..xk] and one output (result) variable [y], find coefficients [b0..bk] closest to
         * y = b0 + b1*x1 + b2*x2 + ... + bk*xk + e (where e is noise)
         * 
         * Y = XB + E           - write down all input in matrix form (n rows), where X is (1,x11..x1k; 1,x21..x2k; ..; 1,xn1..xnk) - 1s are added for b0
         * `B = min S(B)        - need approximate `B that fit the most => need to solve minimization problem for a "cost" function S(B)
         * S = ||Y - X`B||^2    - most typical cost function comes from ordinary least squares (OSL) method: sum[1..n](yi - `b0 - sum[1..k](`bj*xij)), i.e. all the differences between ideal y and target `y
         *          
         * closed-form solution:
         * dS/dB = 0 (partial derivative of cost w.r.t. B equal 0 = solution to minimization problem)
         * E = Y - X`B; S = Et*E; d(Et*E)/dB = 0; ...calculus... ; `B = (Xt * X)' * Xt * Y
         * 
         * gradient descent:
         * 
        */

        public static double[][] LinearCF(double[][] X, double[][] Y)
        {
            //`B = (Xt * X)' * Xt * Y
            var X1 = Matrix.AddCol1(X); //add a column of 1s to the left to represent b0 (intercept)
            var XT = Matrix.Transpose(X1);
            var B = Matrix.Multiply(Matrix.Multiply(Matrix.Invert(Matrix.Multiply(XT, X1)), XT), Y);
            return B;
        }

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
                //2 * AT * (A*X - B)
                //var AX = Matrix.Multiply(A, Matrix.ArrayToVector(X));
                //var AXB = Matrix.Substract(AX, B);
                //var AT2 = Matrix.Scale(Matrix.Transpose(A), 2);
                //derivative = Matrix.Multiply(AT2, AXB);

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

        public static void Test1()
        {
            var X = Input.ReadCSV11("..\\..\\Advertising.csv");
            var B1 = Regression.LinearCF(Matrix.GetCol(X, 0), Matrix.GetCol(X, 3));
            var B2 = Regression.LinearCF(Matrix.GetCol(X, 1), Matrix.GetCol(X, 3));
            var B3 = Regression.LinearCF(Matrix.GetCol(X, 2), Matrix.GetCol(X, 3));
            var B = Regression.LinearCF(Matrix.GetCols(X, new int[] { 0, 1, 2 }), Matrix.GetCol(X, 3));

            Matrix.Print(B1); //TV -> b0 = 7.03, b1 = 0.0475
            Matrix.Print(B2); //radio -> b0 = 9.312, b1 = 0.203
            Matrix.Print(B3); //newsp -> b0 = 12.351, b1 = 0.055
            Matrix.Print(B); //all 3 -> b0 = 2.939, TV = 0.046, radio = 0.189, np = -0.001
        }

        public static void Test2()
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
