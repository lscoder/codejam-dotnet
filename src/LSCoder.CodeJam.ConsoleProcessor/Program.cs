using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
            var input = scanner.ReadLongArray();
            var A = input[0];
            var B = input[1];
            var K = input[2];

            var count = 0;

            // Small file only
            for (int a = 0; a < A; a++)
            {
                for (int b = 0; b < B; b++)
                {
                    if ((a & b) < K)
                        count++;
                }
            }

            return count.ToString();
        }

        private static void WriteResult(int testCaseId, string result, TextWriter outputFile)
        {
            var formatedLine = string.Format("Case #{0}: {1}", testCaseId, result);

            Trace.WriteLine(formatedLine);
            outputFile.WriteLine(formatedLine);
        }
    }
}
