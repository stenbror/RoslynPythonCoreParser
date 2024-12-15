namespace RoslynPythonCoreParser;

public abstract record Token(uint Start, uint End, Trivia[] Trivia);