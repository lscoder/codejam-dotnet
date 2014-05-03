using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
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

            var scanner = fileManager.OpenInputFileScanner(problemInputFile);
            var outputFile = fileManager.CreateOutputFile(problemInputFile);

            Solve(scanner, outputFile);

            scanner.Dispose();
            outputFile.Dispose();

            Trace.WriteLine(string.Format("\nFile `{0}` created!", problemInputFile.Name));

            Console.ReadKey(true);
        }

        private static void Solve(Scanner scanner, TextWriter outputFile)
        {
            var testCasesCount = scanner.ReadInt();

            Trace.WriteLine(string.Format("\nRunning `{0}` test cases...\n", testCasesCount));

            for (var testCaseId = 1; testCaseId <= testCasesCount; testCaseId++)
            {
                var result = Solve(scanner);
                WriteResult(testCaseId, result, outputFile);
            }
        }

        private static string Solve(Scanner scanner)
        {
            var count = scanner.ReadInt();
            var lines = new string[count];
            var invalid = false;

            var baseStr = "";
            for (var i = 0; i < count; i++)
            {
                var line = scanner.ReadLine();
                lines[i] = line;

                var singleLetters = Regex.Replace(lines[i], @"([a-z])\1+", @"$1");

                if (i == 0)
                    baseStr = singleLetters;
                else if (singleLetters != baseStr)
                    invalid = true;
            }

            if(invalid)
                return "Fegla Won";

            var chs = new int[count, baseStr.Length];
            for (int i = 0; i < count; i++)
            {
                var line = lines[i];

                var idx = 0;
                var chIndex = 0;
                while (idx < line.Length)
                {
                    var counter = 1;
                    idx++;

                    while (idx < line.Length)
                    {
                        if(line[idx] == line[idx - 1])
                        {
                            idx++;
                            counter++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    chs[i, chIndex++] = counter;
                }
            }


            var moves = 0;

            for (int chIdx = 0; chIdx < baseStr.Length; chIdx++)
            {
                int min = chs[0, chIdx];
                int max = chs[0, chIdx];

                for (int i = 0; i < count; i++)
                {
                    if (chs[i, chIdx] < min)
                        min = chs[i, chIdx];
                    if (chs[i, chIdx] > max)
                        max = chs[i, chIdx];
                }

                var minMove = Int32.MaxValue;
                for (var j = min; j <= max; j++)
                {
                    var m = 0;
                    for (var k = 0; k < count; k++)
                    {
                        m += Math.Abs(j - chs[k, chIdx]);
                    }

                    minMove = Math.Min(minMove, m);
                }

                moves += minMove;
            }

            return moves.ToString();
        }

        private static void WriteResult(int testCaseId, string result, TextWriter outputFile)
        {
            var formatedLine = string.Format("Case #{0}: {1}", testCaseId, result);

            Trace.WriteLine(formatedLine);
            outputFile.WriteLine(formatedLine);
        }
    }
}
