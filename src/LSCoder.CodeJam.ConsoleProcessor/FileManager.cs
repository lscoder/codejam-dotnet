using System.Collections.Generic;
using System.IO;
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

        public TextWriter CreateOutputFile(ProblemInputFile problemInputFile)
        {
            var fileName = Path.GetFileNameWithoutExtension(problemInputFile.Name) + CodeJamSettings.Default.OutputFileExtension;
            var filePath = Path.GetFullPath(Path.Combine(GetApplicationPath(), CodeJamSettings.Default.OutputFilePath));
            var fullFileName = Path.Combine(filePath, fileName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
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
