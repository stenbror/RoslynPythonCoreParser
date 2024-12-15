namespace RoslynPythonCoreParser;

public abstract record Token(uint Start, uint End, Trivia[] Trivia);

public sealed record BinaryOperatorPlusToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);



public sealed record ColonAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);


public sealed record NumberToken(uint Start, uint End, string Value, Trivia[] Trivia) : Token(Start, End, Trivia);