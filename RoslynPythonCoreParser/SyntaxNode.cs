﻿namespace RoslynPythonCoreParser;

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







/* Statement nodes */
public abstract record StmtNode(uint Start, uint End) : SyntaxNode(Start, End);
