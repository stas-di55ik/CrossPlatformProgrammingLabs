using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CPP_Lab2
{
    public class PalindromeCounter
    {
        [Option(ShortName = "i")]
        public string InputFile { get; }

        [Option(ShortName = "o")]
        public string OutputFile { get; }

        StreamReader inputReader;
        StreamWriter outputWriter;

        static string inputString;
        static long[,] tempMatrix;

        static void InitTempMatrix(int length)
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    tempMatrix[i, j] = -1;
                }
            }
        }

        static long CountPalindromicSubstrings(int start, int end)
        {
            if (start > end) 
            {
                return 0;  // Пуста підстрічка має 0 паліндромних підстрічок
            }

            if (start == end) 
            {
                return tempMatrix[start, end] = 1;  // Один символ - одна паліндромна підстрічка.
            }

            if (tempMatrix[start, end] != -1)
            {
                return tempMatrix[start, end];  // Якщо результат вже обчислений, повертаємо його.
            }

            if (inputString[start] == inputString[end])
            {
                tempMatrix[start, end] = CountPalindromicSubstrings(start + 1, end) 
                    + CountPalindromicSubstrings(start, end - 1) + 1;
                // Якщо символи на кінцях рядка співпадають, додаємо 1 і обчислюємо підстрічки без цих символів.

                Console.WriteLine($"Found palindrome at indices {start} to {end}. Count: {tempMatrix[start, end]}");
            }

            else
            {
                tempMatrix[start, end] = CountPalindromicSubstrings(start + 1, end)
                    + CountPalindromicSubstrings(start, end - 1) 
                    - CountPalindromicSubstrings(start + 1, end - 1);
                // Якщо символи не співпадають, вираховуємо кількість підстрічок без одного з цих символів.

                Console.WriteLine($"Found non-palindrome at indices {start} to {end}. Count: {tempMatrix[start, end]}");
            }

            return tempMatrix[start, end];
        }

        public static void Main(string[] args)
            => CommandLineApplication.Execute<PalindromeCounter>(args);

        private void OnExecute()
        {
            inputReader = new StreamReader(InputFile);
            inputString = inputReader.ReadLine();
            try 
            {
                if (inputString.Length < 1 || inputString.Length > 30) 
                {
                    throw new Exception("Incorrect length of input string");
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
            inputReader.Close();

            Console.WriteLine($"Input string: {inputString}\n");
            int length = inputString.Length;
            tempMatrix = new long[length, length];

            InitTempMatrix(length);

            outputWriter = new StreamWriter(OutputFile);
            outputWriter.WriteLine(CountPalindromicSubstrings(0, length - 1));
            outputWriter.Close();
        }
    }
}
