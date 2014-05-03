using System;
using System.IO;
using System.Linq;
using System.Text;

namespace LSCoder.CodeJam.ConsoleProcessor
{
    public class Scanner : IDisposable
    {
        #region Fields

        private readonly TextReader _fileReader;
        private bool _disposed;

        #endregion

        #region Constructors

        public Scanner(TextReader fileReader)
        {
            _fileReader = fileReader;
        }

        ~Scanner()
        {
            Dispose(false);
        }

        #endregion

        #region Public Methods

        public string ReadLine()
        {
            return _fileReader.ReadLine();
        }

        public string[] ReadLineTokens()
        {
            return ReadLine().Split(' ');
        }

        public char ReadChar()
        {
            return (char)_fileReader.Read();
        }

        public char[] CharArray()
        {
            return ReadLine().ToCharArray();
        }

        public int ReadInt()
        {
            return Int32.Parse(ReadLine());
        }

        public int[] ReadIntArray()
        {
            return ReadLineTokens().Select(Int32.Parse).ToArray();
        }

        public long ReadLong()
        {
            return Int64.Parse(ReadLine());
        }

        public long[] ReadLongArray()
        {
            return ReadLineTokens().Select(Int64.Parse).ToArray();
        }

        public double ReadDouble()
        {
            return Double.Parse(ReadLine());
        }

        public double[] ReadDoubleArray()
        {
            return ReadLineTokens().Select(Double.Parse).ToArray();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        
        public void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                _fileReader.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
