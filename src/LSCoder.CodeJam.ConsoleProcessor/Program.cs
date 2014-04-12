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

            var problemInputFile = inputFileSelector.Select();
            if (problemInputFile == null)
                return;

            var inputFile = fileManager.OpenInputFile(problemInputFile);
            var outputFile = fileManager.CreateOutputFile(problemInputFile);

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
            var values = new Dictionary<int, bool>();

            var firstRow = Int32.Parse(inputFile.ReadLine());
            for (int i = 1; i <= 4; i++)
            {
                var line = inputFile.ReadLine();
                if (i == firstRow)
                {
                    foreach (var value in line.Split(' '))
                    {
                        values.Add(Int32.Parse(value), false);
                    }
                }
            }

            var secondRow = Int32.Parse(inputFile.ReadLine());
            for (int i = 1; i <= 4; i++)
            {
                var line = inputFile.ReadLine();
                if (i == secondRow)
                {
                    foreach (var value in line.Split(' '))
                    {
                        var intValue = Int32.Parse(value);
                        if(values.ContainsKey(intValue))
                        {
                            values[intValue] = true;
                        }
                    }
                }
            }

            var count = 0;
            var gotIt = 0;
            foreach (var keyValue in values)
            {
                if(keyValue.Value)
                {
                    count++;
                    gotIt = keyValue.Key;
                }
            }

            if (count == 0)
                result = "Volunteer cheated!";
            else if (count == 1)
                result = gotIt.ToString();
            else
                result = "Bad magician!";

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
