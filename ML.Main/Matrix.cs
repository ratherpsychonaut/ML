using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Main
{
    static class Matrix
    {
        //for LU-decomposition step-by-step, double-checking the inverse, and many other cool things, see https://matrixcalc.org/en/

        public static double[][] Solve(double[][] A, double[][] B)
        {
            var n = A.Length;
            var P = new int[n]; for (int i = 0; i < n; i++) P[i] = i;
            var LU = new double[n][]; for (int i = 0; i < n; i++) LU[i] = new double[n];
            var X = new double[n][]; for (int i = 0; i < n; i++) X[i] = new double[n];
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) LU[i][j] = A[i][j]; //start with LU = A

            //LU decomposition: use Gaussian elimination to make U (substract rows until lower left triangle turns to 0s)
            //then L will be multipliers we used along the way + 1s on the diagonal
            //and we store both L and U in one matrix for convenience

            //just one thing: since we divide by A[k][k] when calculating a multiplier, that A[k][k] can't be zero
            //if it's zero, we need to swap row[k] with one of the lower rows (where it's not zero) first
            //this is called "partial pivoting" (partial because swapping only rows and not rows + columns)
            //we'll use P to store info about swaps so we can restore the original order afterwards

            //futhermore, it is considered to improve numerical stability if we divide by the biggest absolute number available
            //so we can incorporate both by first finding max among A[k..n][k] elements and, if max is not already at row k, swapping
            //after that, if A[k][k] is still 0, then everything below is also 0, so we can skip the column k altogether

            for (int k = 0; k < n - 1; k++)
            {
                var max = Math.Abs(LU[k][k]); var imax = k;
                for (int i = k + 1; i < n; i++) if (Math.Abs(LU[i][k]) > max) { max = Math.Abs(LU[i][k]); imax = i; }
                if (imax != k)
                {
                    var v = LU[k]; LU[k] = LU[imax]; LU[imax] = v; //swapping rows
                    var a = P[k]; P[k] = P[imax]; P[imax] = a; //recording this swap in P
                }

                if (LU[k][k] != 0)
                    for (int i = k + 1; i < n; i++)
                    {
                        var m = LU[i][k] / LU[k][k];                          //multiplier
                        for (int j = k; j < n; j++) LU[i][j] -= m * LU[k][j]; //row substraction - getting U elements
                        LU[i][k] = m;                                         //putting multiplier where we now have 0 - getting L element
                    }
            }

            //Ax = b                            - so this is a linear system of equations (x, b are vectors)
            //AX = B  -> Axi = bi, i = 1..k     - if X, B are matrices with k columns, we can just solve it for each pair of columns separately
            //LUX = B -> { LY = B; UX = Y }     - if A is decomposed into lower and upper triangular matrices, we san split the system in 2 parts, both of which are easily solvable by substitution

            for (int k = 0; k < n; k++) //for each pair of columns
            {
                for (int i = 0; i < n; i++) //solving Ly = b, going top to bottom (forward substitution) [gonna store Y in X too]
                {
                    double z = B != null ? B[P[i]][k] : (P[i] == k ? 1 : 0); //using B with the same row swaps (essentially solving PLUX = PB) - or, if B is null, using I and finding inverse instead
                    for (int j = 0; j < i; j++) z -= LU[i][j] * X[j][k];     //moving all other elements (with y's we already know) to the right side
                    X[i][k] = z / 1;                                         //1s because L has 1s on the diagonal
                }
                for (int i = n - 1; i >= 0; i--) //solving Ux = y, going bottom to top (backward substitution)
                {
                    double z = X[i][k];
                    for (int j = n - 1; j > i; j--) z -= LU[i][j] * X[j][k];
                    X[i][k] = z / LU[i][i];
                }
            }

            return X;
        }

        public static double[][] Invert(double[][] A)
        {
            //in AX = B, if B = I (identity matrix), that means X is inverse A
            //so inverting is pretty much the same as solving a system
            //of course, allocating memory for n x n matrix when it can be described by "if (i == j) then 1 else 0" is a bit wasteful,
            //so instead of calling Solve(A, I) we'll call Solve(A, null), treating "null" inside Solve as a signal to use I
            //so this method is just a wrapper, really

            var X = Solve(A, null);
            return X;
        }

        public static double[][] Multiply(double[][] A, double[][] B)
        {
            var m = A[1].Length;
            var n = A.Length;
            var p = B[1].Length;

            if (B.Length != m) { Console.WriteLine("ERROR: can't multiply matrices A and B because cols(A) != rows(B)"); Environment.Exit(0); }

            var C = new double[n][]; for (int i = 0; i < n; i++) C[i] = new double[p];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < p; j++)
                {
                    C[i][j] = 0;
                    for (int k = 0; k < m; k++) C[i][j] += A[i][k] * B[k][j];
                }

            return C;
        }

        public static double[][] Transpose(double[][] A)
        {
            var m = A[1].Length;
            var n = A.Length;
            var T = new double[m][]; for (int i = 0; i < m; i++) T[i] = new double[n];
            for (int i = 0; i < m; i++) for (int j = 0; j < n; j++) T[i][j] = A[j][i];
            return T;
        }

        public static void Print(double[][] A, int precision = -1)
        {
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[i].Length; j++)
                {
                    var s = precision == -1 ? A[i][j].ToString() : A[i][j].ToString("0." + new string('0', precision));
                    Console.Write(s + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static double[][] AddCol1(double[][] A)
        {
            var m = A[0].Length;
            var n = A.Length;
            var A1 = new double[n][]; for (int i = 0; i < n; i++) A1[i] = new double[m + 1];
            for (int i = 0; i < n; i++) for (int j = 0; j <= m; j++) A1[i][j] = j == 0 ? 1 : A[i][j - 1];
            return A1;
        }

        public static double[][] GetCol(double[][] A, int k)
        {
            return GetCols(A, new int[] { k });
        }
        public static double[][] GetCols(double[][] A, int[] c)
        {
            var n = A.Length;
            var m = c.Length;
            var B = new double[n][];
            for (int i = 0; i < n; i++)
            {
                B[i] = new double[m]; int k = 0;
                for (int j = 0; j < A[i].Length; j++) if (c.Contains(j)) { B[i][k] = A[i][j]; k++; }
            }
            return B;
        }
        /*public static double[] ColumnToVector(double[][] A, int k = 0)
		{
			var n = A.Length;
			var V = new double[n];
			for (int i = 0; i < n; i++) V[i] = A[i][k];
			return V;
		}*/
    }
}