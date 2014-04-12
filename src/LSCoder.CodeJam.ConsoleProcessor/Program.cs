using System;
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
            /************************** YOUR CODE HERE **************************/

            WriteResult(testCaseId, "Your result here", outputFile);
        }

        private static void WriteResult(int testCaseId, string result, TextWriter outputFile)
        {
            var formatedLine = string.Format("Case #{0}: {1}", testCaseId, result);

            Trace.WriteLine(formatedLine);
            outputFile.WriteLine(formatedLine);
        }
    }
}
