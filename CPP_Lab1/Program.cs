using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CPP_Lab1 
{
    public class WordSquareBuilder
    {
        [Option(ShortName = "i")]
        public string InputFile { get; }

        [Option(ShortName = "o")]
        public string OutputFile { get; }

        int squareSize;
        bool[] used;
        string[] words;
        char[][][] squares;
        StreamReader inputReader;
        StreamWriter outputWriter;

        bool IsWordPlacementValid(int currentSquare, int currentRow, int wordIndex)
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

        void PlaceWord(int currentSquare, int currentRow, int wordIndex)
        {
            for (int column = 0; column < squareSize; column++)
            {
                squares[currentSquare][currentRow][column] = words[wordIndex][column];
                squares[currentSquare][column][currentRow] = words[wordIndex][column];
            }
            used[wordIndex] = true;
        }

        void RemoveWord(int currentSquare, int currentRow, int wordIndex)
        {
            for (int column = currentRow; column < squareSize; column++)
            {
                squares[currentSquare][currentRow][column] = ' ';
                squares[currentSquare][column][currentRow] = ' ';
            }
            used[wordIndex] = false;
        }

        bool BuildWordSquareRecursive(int currentSquare, int currentRow)
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

        void PrintSquares()
        {
            for (int square = 0; square < 2; square++)
            {
                for (int row = 0; row < squareSize; row++)
                {
                    outputWriter.WriteLine(new string(squares[square][row]));
                }
                outputWriter.WriteLine();
            }
        }

        void DisplaySquares()
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

        public static void Main(string[] args)
            => CommandLineApplication.Execute<WordSquareBuilder>(args);

        private void OnExecute() 
        {
            inputReader = new StreamReader(InputFile);
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
            

            outputWriter = new StreamWriter(OutputFile);
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

            PrintSquares();
            outputWriter.Close();
        }
    }
}
