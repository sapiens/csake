using System;

namespace CSake.Internals
{
    public interface IReadLine:IDisposable
    {
        string ReadLine();
        bool HasFinished { get; }
    }
}