using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryLab4
{
    public class ClassLab1
    {
        public static int squareSize;
        public static bool[] used;
        public static string[] words;
        public static char[][][] squares;

        public static bool IsWordPlacementValid(int currentSquare, int currentRow, int wordIndex)
        {
            for (int column = 0; column < currentRow; column++)
            {
                if (squares[currentSquare][currentRow][column] != words[wordIndex][column])
                {
                    return false;
                }
            }
            return true;
        }

        public static void PlaceWord(int currentSquare, int currentRow, int wordIndex)
        {
            for (int column = 0; column < squareSize; column++)
            {
                squares[currentSquare][currentRow][column] = words[wordIndex][column];
                squares[currentSquare][column][currentRow] = words[wordIndex][column];
            }
            used[wordIndex] = true;
        }

       public static void RemoveWord(int currentSquare, int currentRow, int wordIndex)
        {
            for (int column = currentRow; column < squareSize; column++)
            {
                squares[currentSquare][currentRow][column] = ' ';
                squares[currentSquare][column][currentRow] = ' ';
            }
            used[wordIndex] = false;
        }

        public static bool BuildWordSquareRecursive(int currentSquare, int currentRow)
        {
            DisplaySquares();

            if (currentRow == squareSize)
            {
                return true;
            }

            for (int wordIndex = 0; wordIndex < 2 * squareSize; wordIndex++)
            {
                if (!used[wordIndex] && IsWordPlacementValid(currentSquare, currentRow, wordIndex))
                {
                    PlaceWord(currentSquare, currentRow, wordIndex);

                    if (BuildWordSquareRecursive(1 - currentSquare, currentRow + currentSquare))
                    {
                        return true;
                    }

                    RemoveWord(currentSquare, currentRow, wordIndex);
                }
            }
            return false;
        }

        public static void PrintSquares(StreamWriter writer)
        {
            for (int square = 0; square < 2; square++)
            {
                for (int row = 0; row < squareSize; row++)
                {
                    writer.WriteLine(new string(squares[square][row]));
                }
                writer.WriteLine();
            }
        }

        public static void DisplaySquares()
        {
            for (int square = 0; square < 2; square++)
            {
                for (int row = 0; row < squareSize; row++)
                {
                    Console.WriteLine(new string(squares[square][row]));
                }
                Console.WriteLine();
            }
        }

        public static void Execute(string InputFile, string OutputFile)
        {
            StreamReader inputReader = new StreamReader(InputFile);
            StreamWriter outputWriter = new StreamWriter(OutputFile);

            try
            {
                squareSize = int.Parse(inputReader.ReadLine());
                if (squareSize < 2 || squareSize > 10)
                {
                    throw new Exception("Incorrect square size value");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            words = new string[2 * squareSize];
            try
            {
                for (int wordIndex = 0; wordIndex < 2 * squareSize; wordIndex++)
                {
                    words[wordIndex] = inputReader.ReadLine();
                    if (words[wordIndex].Length != squareSize)
                    {
                        throw new Exception("Incorrect number of words or its length");
                    }
                }
                inputReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            squares = new char[2][][];
            for (int currentSquare = 0; currentSquare < 2; currentSquare++)
            {
                squares[currentSquare] = new char[squareSize][];
                for (int row = 0; row < squareSize; row++)
                {
                    squares[currentSquare][row] = new char[squareSize];
                    for (int column = 0; column < squareSize; column++)
                    {
                        squares[currentSquare][row][column] = ' ';
                    }
                }
            }

            used = new bool[2 * squareSize];

            BuildWordSquareRecursive(0, 0);

            PrintSquares(outputWriter);
            outputWriter.Close();
        }
    }
}