namespace RoslynPythonCoreParser;

public abstract record Token(uint Start, uint End, Trivia[] Trivia);

/* Reserved keywords in Python 3.13 */
public sealed record FalseToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record NoneToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record TrueToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record AndToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record AsToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record AssertToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record AsyncToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record AwaitToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BreakToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ClassToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ContinueToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record DefToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record DelToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ElifToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ElseToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ExceptToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record FinallyToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ForToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record FromToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record GlobalToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record IfToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ImportToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record InToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record IsToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record LambdaToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record NonlocalToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record NotToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record OrToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record PassToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record RaiseToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ReturnToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record TryToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record WhileToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record WithToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record YieldToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);


public sealed record BinaryOperatorPlusToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);



public sealed record ColonAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);


public sealed record NumberToken(uint Start, uint End, string Value, Trivia[] Trivia) : Token(Start, End, Trivia);