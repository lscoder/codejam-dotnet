using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LSCoder.CodeJam.ConsoleProcessor
{
    public class FileManager
    {
        #region Private Methods

        private string GetApplicationPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        private string GetOutputFileName()
        {
            var fileName = CodeJamSettings.Default.OutputFileName;
            
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.fff") + ".out";

            return fileName;
        }

        #endregion

        #region Public Methods

        public IDictionary<int, ProblemInputFile> ScanInputFiles()
        {
            var id = 1;
            var path = Path.GetFullPath(Path.Combine(GetApplicationPath(), CodeJamSettings.Default.InputFilePath));
            var inputFiles = new Dictionary<int, ProblemInputFile>();

            foreach (var filePath in Directory.GetFiles(path))
            {
                var fileName = Path.GetFileName(filePath);

                if ((fileName == null) || fileName.StartsWith("."))
                    continue;

                var inputFile = new ProblemInputFile(id++, Path.GetFileName(filePath), filePath);
                inputFiles.Add(inputFile.Id, inputFile);
            }

            return inputFiles;
        }

        public TextReader OpenInputFile(ProblemInputFile problemInputFile)
        {
            return new StreamReader(new FileStream(problemInputFile.Path, FileMode.Open, FileAccess.Read));
        }

        public TextWriter CreateOutputFile()
        {
            var fileName = GetOutputFileName();
            var filePath = Path.GetFullPath(Path.Combine(GetApplicationPath(), CodeJamSettings.Default.OutputFilePath));
            var fullFileName = Path.Combine(filePath, fileName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if(CodeJamSettings.Default.ClearOutputDirectory)
            {
                foreach (var file in Directory.GetFiles(filePath))
                {
                    if ((Path.GetFileName(file)).StartsWith("."))
                        continue;

                    File.Delete(file);
                }
            }

            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }

            return new StreamWriter(fullFileName, false, Encoding.ASCII);
        }

        #endregion
    }
}
