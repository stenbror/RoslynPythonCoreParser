namespace RoslynPythonCoreParser;

public class SyntaxError(uint pos, string text) : Exception
{
    public uint Location { get; init; } = pos;
    public string Text { get; init; } = text;
}