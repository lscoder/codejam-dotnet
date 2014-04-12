using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace LSCoder.CodeJam.ConsoleProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

            var fileManager = new FileManager();
            var inputFileSelector = new AsciiMenuFileSelector(fileManager, Console.In, Console.Out);

            var problemInputFile = inputFileSelector.Select();
            if (problemInputFile == null)
                return;

            var inputFile = fileManager.OpenInputFile(problemInputFile);
            var outputFile = fileManager.CreateOutputFile(problemInputFile);

            Solve(inputFile, outputFile);

            inputFile.Dispose();
            outputFile.Dispose();

            Trace.WriteLine(string.Format("\nFile `{0}` created!", problemInputFile.Name));

            Console.ReadKey(true);
        }

        private static void Solve(TextReader inputFile, TextWriter outputFile)
        {
            var testCasesCount = Int32.Parse(inputFile.ReadLine());

            Trace.WriteLine(string.Format("\nRunning `{0}` test cases...\n", testCasesCount));

            for (var i = 1; i <= testCasesCount; i++)
            {
                SolveTestCase(i, inputFile, outputFile);
            }
        }

        private static void SolveTestCase(int testCaseId, TextReader inputFile, TextWriter outputFile)
        {
            var tokens = inputFile.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
            var rows = tokens[0];
            var cols = tokens[1];
            var mines = tokens[2];
            var board = new char[rows, cols];
            var result = "Impossible";

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    board[r, c] = '*';

            if (Click(board, rows, cols, mines))
                result = GetString(board, rows, cols);

            WriteResult(testCaseId, "\n" + result, outputFile);
        }

        private static bool Click(char[,] board, int rows, int cols, int mines)
        {
            var expand = (rows * cols) - mines;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (Expand(board, rows, cols, r, c, r, c, expand, expand, 'c'))
                        return true;
                }
            }

            return false;
        }

        private static bool Expand(char[,] board, int rows, int cols, int sr, int sc, int cr, int cc, int nonMines, int remaining, char marker = '.')
        {
            if ((cr < 0) || (cr >= rows) || (cc < 0) || (cc >= cols) || (board[cr, cc] != '*'))
                return false;

            var previousValue = board[cr, cc];

            board[cr, cc] = marker;
            remaining--;

            if (remaining == 0)
            {
                if (IsValid(board, rows, cols, sr, sc, nonMines))
                    return true;
            }
            else
            {
                if (Expand(board, rows, cols, sr, sc, cr - 1, cc, nonMines, remaining))
                    return true;
                if (Expand(board, rows, cols, sr, sc, cr + 1, cc, nonMines, remaining))
                    return true;
                if (Expand(board, rows, cols, sr, sc, cr, cc + 1, nonMines, remaining))
                    return true;
                if (Expand(board, rows, cols, sr, sc, cr, cc - 1, nonMines, remaining))
                    return true;
            }

            board[cr, cc] = previousValue;

            return false;
        }

        private static bool IsValid(char[,] board, int rows, int cols, int sr, int sc, int nonMines)
        {
            var cache = new int[rows, cols];
            var visited = new bool[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    visited[r, c] = false;

                    if (board[r, c] == '*')
                        cache[r, c] = -1;
                    else
                        cache[r, c] = CountMines(board, rows, cols, r, c);
                }
            }

            var opened = 0;
            var queue = new Queue<Cell>();

            queue.Enqueue(new Cell(sr, sc));
            while (queue.Count > 0)
            {
                var cell = queue.Dequeue();

                if ((cell.Row < 0) || (cell.Row >= rows) || (cell.Col < 0) || (cell.Col >= cols) || visited[cell.Row, cell.Col])
                    continue;

                opened++;

                visited[cell.Row, cell.Col] = true;

                if (cache[cell.Row, cell.Col] == 0)
                {
                    queue.Enqueue(new Cell(cell.Row - 1, cell.Col));
                    queue.Enqueue(new Cell(cell.Row - 1, cell.Col + 1));
                    queue.Enqueue(new Cell(cell.Row, cell.Col + 1));
                    queue.Enqueue(new Cell(cell.Row + 1, cell.Col + 1));
                    queue.Enqueue(new Cell(cell.Row + 1, cell.Col));
                    queue.Enqueue(new Cell(cell.Row + 1, cell.Col - 1));
                    queue.Enqueue(new Cell(cell.Row, cell.Col - 1));
                    queue.Enqueue(new Cell(cell.Row - 1, cell.Col - 1));
                }
            }

            return opened == nonMines;
        }

        private static int CountMines(char[,] board, int rows, int cols, int r, int c)
        {
            var mines = 0;

            mines += HasMine(board, rows, cols, r - 1, c) ? 1 : 0;
            mines += HasMine(board, rows, cols, r - 1, c + 1) ? 1 : 0;
            mines += HasMine(board, rows, cols, r, c + 1) ? 1 : 0;
            mines += HasMine(board, rows, cols, r + 1, c + 1) ? 1 : 0;
            mines += HasMine(board, rows, cols, r + 1, c) ? 1 : 0;
            mines += HasMine(board, rows, cols, r + 1, c - 1) ? 1 : 0;
            mines += HasMine(board, rows, cols, r, c - 1) ? 1 : 0;
            mines += HasMine(board, rows, cols, r - 1, c - 1) ? 1 : 0;

            return mines;
        }

        private static bool HasMine(char[,] board, int rows, int cols, int r, int c)
        {
            if ((r >= 0) && (c >= 0) && (r < rows) && (c < cols) && (board[r, c] == '*'))
                return true;

            return false;
        }

        private static string GetString(char[,] board, int rows, int cols)
        {
            var stringBuilder = new StringBuilder();

            for (int r = 0; r < rows; r++)
            {
                if (r > 0)
                    stringBuilder.AppendLine();

                for (int c = 0; c < cols; c++)
                {
                    stringBuilder.Append(board[r, c]);
                }
            }

            return stringBuilder.ToString();
        }

        private static void WriteResult(int testCaseId, string result, TextWriter outputFile)
        {
            var formatedLine = string.Format("Case #{0}: {1}", testCaseId, result);

            Trace.WriteLine(formatedLine);
            outputFile.WriteLine(formatedLine);
        }
    }

    public struct Cell
    {
        public Cell(int row, int col)
            : this()
        {
            Row = row;
            Col = col;
        }

        public int Row { get; set; }

        public int Col { get; set; }
    }
}
