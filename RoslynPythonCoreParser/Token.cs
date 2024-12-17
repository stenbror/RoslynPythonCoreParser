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

/* Operator or delimiter tokens */
public sealed record BinaryOperatorPlusToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorMinusToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorMulToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorPowerToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorDivToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorFloorDivToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorModuloToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorMatricesToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorShiftLeftToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorShiftRightToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorBitAndToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorBitOrToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BinaryOperatorBitXorToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record UnaryOperatorBitInvertToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ColonAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record CompareLessToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record CompareGreaterToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record CompareLessEqualToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record CompareGreaterEqualToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record CompareEqualToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record CompareNotEqualToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record LeftParenToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record LeftBracketToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record LeftCurlyToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record RightParenToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record RightBracketToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record RightCurlyToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record CommaToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ColonToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record DotToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record SemiColonToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record AssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ArrowToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record PlusAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record MinusAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record MulAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record DivAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record FloorDivAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ModuloAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record MatricesAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BitAndAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BitOrAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record BitXorAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ShiftLeftAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ShiftRightAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record PowerAssignToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record ElipsisToken(uint Start, uint End, Trivia[] Trivia) : Token(Start, End, Trivia);

/* Literal tokens */
public sealed record NumberToken(uint Start, uint End, string Value, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record NameToken(uint Start, uint End, string Value, Trivia[] Trivia) : Token(Start, End, Trivia);
public sealed record StringToken(uint Start, uint End, string Value, Trivia[] Trivia) : Token(Start, End, Trivia);


/* Special tokens */
public sealed record NewlineToken(uint Start, uint End, Trivia[] Trivia, char Ch1, char ch2) : Token(Start, End, Trivia);