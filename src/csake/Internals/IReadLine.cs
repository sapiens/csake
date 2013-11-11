namespace CSake.Internals
{
    public interface IReadLine
    {
        string ReadLine();
        bool HasFinished { get; }
    }
}