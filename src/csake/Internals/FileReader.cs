using System.IO;

namespace CSake.Internals
{
    public class FileReader : IReadLine
    {
        private readonly string _filename;
        private StreamReader _reader;

        public FileReader(string filename)
        {
            _filename = filename;
            _reader = File.OpenText(_filename);
        }

        public string ReadLine()
        {
            return _reader.ReadLine();

        }

        public bool HasFinished
        {
            get
            {
                return _reader.EndOfStream;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _reader.Dispose();
            _reader = null;
        }
    }
}