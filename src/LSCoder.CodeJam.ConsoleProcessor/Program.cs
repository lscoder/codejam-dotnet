using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LSCoder.CodeJam.ConsoleProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileManager = new FileManager();
            var inputFileSelector = new AsciiMenuFileSelector(fileManager, Console.In, Console.Out);

            var inputFile = inputFileSelector.Select();
            if (inputFile == null)
                return;

            var outputFile = fileManager.CreateOutputFile();

            Solve(inputFile, outputFile);

            inputFile.Dispose();
            outputFile.Dispose();

            Console.ReadKey(true);
        }

        private static void Solve(TextReader inputFile, TextWriter outputFile)
        {
            var testCaseCount = Int32.Parse(inputFile.ReadLine());
            for(var i = 1; i <= testCaseCount; i++)
            {
                SolveTestCase(i, inputFile, outputFile);
            }
        }

        private static void SolveTestCase(int testCaseId, TextReader inputFile, TextWriter outputFile)
        {
            var result = "";

            WriteResult(testCaseId, result, outputFile);
        }

        private static void WriteResult(int testCaseId, string result, TextWriter outputFile)
        {
            var formatedLine = string.Format("Case #{0}: {1}", testCaseId, result);

            Trace.WriteLine(formatedLine);
            outputFile.WriteLine(formatedLine);
        }
    }
}
