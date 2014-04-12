using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
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
            var tokens = inputFile.ReadLine().Split(' ').Select(Double.Parse).ToList();

            var cps = 2.0;
            var c = tokens[0];
            var f = tokens[1];
            var x = tokens[2];
            var best = Double.MaxValue;
            double elapsedTime = 0;

            do
            {
                var tc = c/cps;
                var tx = x/cps;
                var current = elapsedTime + tx;

                if(current > best)
                    break;

                best = elapsedTime + tx;
                elapsedTime += tc;
                cps += f;
            } while (true);

            WriteResult(testCaseId, best.ToString(), outputFile);
        }

        private static void WriteResult(int testCaseId, string result, TextWriter outputFile)
        {
            var formatedLine = string.Format("Case #{0}: {1}", testCaseId, result);

            Trace.WriteLine(formatedLine);
            outputFile.WriteLine(formatedLine);
        }
    }
}
