using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryLab5
{
    public static class ClassLab2
    {
        public static string str;
        public static long[,] tempMatrix;

        public static void InitTempMatrix(int length)
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    tempMatrix[i, j] = -1;
                }
            }
        }

        public static long CountPalindromicSubstrings(int start, int end)
        {
            if (start > end)
            {
                return 0;
            }

            if (start == end)
            {
                return tempMatrix[start, end] = 1;
            }

            if (tempMatrix[start, end] != -1)
            {
                return tempMatrix[start, end];
            }

            if (str[start] == str[end])
            {
                tempMatrix[start, end] = CountPalindromicSubstrings(start + 1, end) + CountPalindromicSubstrings(start, end - 1) + 1;
                Console.WriteLine($"Found palindrome at indices {start} to {end}. Count: {tempMatrix[start, end]}");
            }
            else
            {
                tempMatrix[start, end] = CountPalindromicSubstrings(start + 1, end)
                    + CountPalindromicSubstrings(start, end - 1)
                    - CountPalindromicSubstrings(start + 1, end - 1);
                Console.WriteLine($"Found non-palindrome at indices {start} to {end}. Count: {tempMatrix[start, end]}");
            }

            return tempMatrix[start, end];
        }

        public static long Execute(string inputString)
        {
            str = inputString;
            try
            {
                if (str.Length < 1 || str.Length > 30)
                {
                    throw new Exception("Incorrect length of input string");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            Console.WriteLine($"Input string: {str}\n");
            int length = str.Length;
            tempMatrix = new long[length, length];

            InitTempMatrix(length);

            return CountPalindromicSubstrings(0, length - 1);
        }
    }
}
