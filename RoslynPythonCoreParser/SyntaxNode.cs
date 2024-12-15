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



/* Statement nodes */
public abstract record StmtNode(uint Start, uint End) : SyntaxNode(Start, End);
