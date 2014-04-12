using System;
using System.Collections.Generic;
using System.IO;

namespace LSCoder.CodeJam.ConsoleProcessor
{
    public class AsciiMenuFileSelector
    {
        #region Field

        private readonly FileManager _fileManager;
        private readonly TextReader _textReader;
        private readonly TextWriter _textWriter;

        #endregion

        #region Constructors

        public AsciiMenuFileSelector(FileManager fileManager, TextReader textReader, TextWriter textWriter)
        {
            _fileManager = fileManager;
            _textReader = textReader;
            _textWriter = textWriter;
        }

        #endregion

        #region Private Methods

        private void ShowMenu(IDictionary<int, ProblemInputFile> files)
        {
            _textWriter.WriteLine("\n\nChoose one of the input files listed below\n");

            foreach (var file in files)
            {
                _textWriter.WriteLine(string.Format("\t[{0}] {1}", file.Key, file.Value.Name));
            }

            _textWriter.WriteLine("\t[0] Sair\n");
            _textWriter.Write("File Id -> ");
        }

        #endregion

        #region Public Methods

        public ProblemInputFile Select()
        {
            var problemInputFiles = _fileManager.ScanInputFiles();
            int fileId;

            do
            {
                ShowMenu(problemInputFiles);
                var option = _textReader.ReadLine();

                if (!Int32.TryParse(option, out fileId))
                    fileId = -1;

                if (fileId == 0)
                    return null;

            } while (!problemInputFiles.ContainsKey(fileId));

            return problemInputFiles[fileId];
        }

        #endregion
    }
}
