using System;

namespace CSake.Internals
{
    public class CodeBlockReader : IReadLine
    {
        private string[] _lines;
        private int _i;

        public CodeBlockReader(string code)
        {
            _lines = code.Split(new[] {"\r\n"}, StringSplitOptions.None);
            _i = -1;
        }
        public string ReadLine()
        {
            if (HasFinished)
            {
                throw new InvalidOperationException("I finished the code");
            }
            _i++;
            return _lines[_i];
        }

        public bool HasFinished
        {
            get
            {
                return _i == _lines.Length - 1;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}