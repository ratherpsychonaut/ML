using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ML.Main
{
    static class Input
    {
        public static double[][] ReadCSV11(string filename, string lineSeparator = null)
        {
            var t = File.ReadAllText(filename);
            var s = t.Split(new string[] { lineSeparator ?? Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var A = new double[s.Length - 1][];
            for (int i = 0; i < s.Length - 1; i++)
            {
                //1st string is headers, so starting with the 2nd
                //1st el in every str is str number, so starting with the 2nd 
                var v = s[i + 1].Split(',');
                A[i] = new double[v.Length - 1];
                for (int j = 0; j < v.Length - 1; j++) A[i][j] = double.Parse(v[j + 1], System.Globalization.CultureInfo.InvariantCulture);
            }
            return A;
        }

        public static double[][] ReadCSV(string filename, string lineSeparator = null)
        {
            var t = File.ReadAllText(filename);
            var s = t.Split(new string[] { lineSeparator ?? Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var A = new double[s.Length][];
            for (int i = 0; i < s.Length; i++)
            {
                var v = s[i].Split(',');
                A[i] = new double[v.Length];
                for (int j = 0; j < v.Length; j++)
                    A[i][j] = double.Parse(v[j], System.Globalization.CultureInfo.InvariantCulture);
            }
            return A;
        }
    }
}
