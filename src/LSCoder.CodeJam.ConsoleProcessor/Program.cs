using System;
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

            for (var testCaseId = 1; testCaseId <= testCasesCount; testCaseId++)
            {
                var result = Solve(inputFile);
                WriteResult(testCaseId, result, outputFile);
            }
        }

        private static string Solve(TextReader inputFile)
        {
            var nl = inputFile.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
            var n = nl[0];
            var l = nl[1];
            var initial = ConvertFlow(inputFile.ReadLine().Split(' '));
            var final = ConvertFlow(inputFile.ReadLine().Split(' '));

            var result = Solve(n, l, initial, final, 0);

            return result == -1 ? "NOT POSSIBLE" : result.ToString();
        }

        public static int Solve(int n, int l, long[] initial, long[] final, int pos)
        {
            if (!IsOk(initial, final, pos))
                return -1;

            if (pos == l)
                return 0;


            var r1 = Solve(n, l, initial, final, pos + 1);

            if (r1 == 0)
                return 0;

            SwitchBit(initial, pos);
            var r2 = Solve(n, l, initial, final, pos + 1);
            SwitchBit(initial, pos);

            if ((r1 == -1) && (r2 == -1))
                return -1;

            if ((r1 != -1) && (r2 != -1))
            {
                if (r1 <= r2)
                    return r1;

                return 1 + r2;
            }

            if (r1 != -1)
                return r1;

            return 1 + r2;
        }

        private static long[] ConvertFlow(string[] input)
        {
            var result = new long[input.Length];

            for (var i = 0; i < input.Length; i++)
                result[i] = Convert.ToInt64(input[i], 2);

            return result;
        }

        private static void SwitchBit(long[] input, int bit)
        {
            for (var i = 0; i < input.Length; i++)
            {
                input[i] = input[i] ^ (1 << bit);
            }
        }

        private static bool IsOk(long[] start, long[] end, int bitCount)
        {
            if (bitCount == 0)
                return true;

            long mask = (1 << bitCount) - 1;
            var s = new long[start.Length];
            var e = new long[end.Length];

            for (int i = 0; i < start.Length; i++)
            {
                s[i] = start[i] & mask;
                e[i] = end[i] & mask;
            }

            Array.Sort(s);
            Array.Sort(e);

            for (int i = 0; i < start.Length; i++)
            {
                if (s[i] != e[i])
                    return false;
            }

            return true;
        }

        private static void WriteResult(int testCaseId, string result, TextWriter outputFile)
        {
            var formatedLine = string.Format("Case #{0}: {1}", testCaseId, result);

            Trace.WriteLine(formatedLine);
            outputFile.WriteLine(formatedLine);
        }
    }
}
