﻿using System;
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
        
        public static void Test()
        {
            var X = Input.ReadCSV11("..\\..\\Advertising.csv");
            var B1 = LinearCF(Matrix.GetCol(X, 0), Matrix.GetCol(X, 3));
            var B2 = LinearCF(Matrix.GetCol(X, 1), Matrix.GetCol(X, 3));
            var B3 = LinearCF(Matrix.GetCol(X, 2), Matrix.GetCol(X, 3));
            var B = LinearCF(Matrix.GetCols(X, new int[] { 0, 1, 2 }), Matrix.GetCol(X, 3));

            Matrix.Print(B1); //TV -> b0 = 7.03, b1 = 0.0475
            Matrix.Print(B2); //radio -> b0 = 9.312, b1 = 0.203
            Matrix.Print(B3); //newsp -> b0 = 12.351, b1 = 0.055
            Matrix.Print(B); //all 3 -> b0 = 2.939, TV = 0.046, radio = 0.189, np = -0.001
        }
    }
}
