namespace RoslynPythonCoreParser;

/* Parent base nodes */
public abstract record SyntaxNode(uint Start, uint End);
public abstract record ExprNode(uint Start, uint End) : SyntaxNode(Start, End);

/* Expression nodes */
public sealed record NamedExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right) 
    : ExprNode(Start, End);

public sealed record TestExprNode(uint Start, uint End, ExprNode Left, Token Symbol1, ExprNode Right, Token Symbol2, ExprNode Next) 
    : ExprNode(Start, End);

public sealed record LambdaExpr(uint Start, uint End, Token Symbol1, ExprNode Left, Token Symbol2, ExprNode Right, bool IsConditional) 
    : ExprNode(Start, End);

public sealed record OrTestExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record AndTestExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record NotTestExprNode(uint Start, uint End, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareLessExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareGreaterExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareEqualExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareGreaterEqualExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareLessEqualExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareNotEqualExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareInExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareNotInExprNode(uint Start, uint End, ExprNode Left, Token Symbol1, Token Symbol2, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareIsExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record CompareIsNotExprNode(uint Start, uint End, ExprNode Left, Token Symbol1, Token Symbol2, ExprNode Right)
    : ExprNode(Start, End);

public sealed record StarExprNode(uint Start, uint End, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record OrExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record XorExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record AndExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record ShiftLeftExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record ShiftRightExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record BinaryOperatorPlusExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record BinaryOperatorMinusExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record BinaryOperatorMulExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record BinaryOperatorMatriceExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record BinaryOperatorDivExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record BinaryOperatorModuloExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record BinaryOperatorFloorDivExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record UnaryOperatorPlusExprNode(uint Start, uint End, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record UnaryOperatorMinusExprNode(uint Start, uint End, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record UnaryOperatorInvertPlusExprNode(uint Start, uint End, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record BinaryOperatorPowerExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record AtomExprNode(uint Start, uint End, Token? Await, ExprNode Right, ExprNode[] Trailer)
    : ExprNode(Start, End);

public sealed record LiteralNameNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralNumberNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralStringNode(uint Start, uint End, Token[] Symbols) : ExprNode(Start, End);

public sealed record LiteralEllipsisNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralNoneNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralTrueNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralFalseNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);





/* Statement nodes */
public abstract record StmtNode(uint Start, uint End) : SyntaxNode(Start, End);
