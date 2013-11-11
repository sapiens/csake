namespace CSake.Internals
{
    public interface IInterpretLine
    {
        LineType Type { get; }
        bool CanInterpret(string line);
    }
}