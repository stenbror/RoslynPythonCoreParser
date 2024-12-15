namespace RoslynPythonCoreParser;

public interface IPythonCoreTokenizer
{
    public Token Symbol { get; set; }
    public uint Position { get; set; }
    
    void Advance();
}


public class PythonCoreTokenizer : IPythonCoreTokenizer
{
    public required Token Symbol { get; set; }
    
    public required uint Position { get; set; }

    public void Advance()
    {
        
    }
}