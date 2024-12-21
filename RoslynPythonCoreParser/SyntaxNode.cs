namespace RoslynPythonCoreParser;

/* Parent base nodes */
public abstract record SyntaxNode(uint Start, uint End);
public abstract record ExprNode(uint Start, uint End) : SyntaxNode(Start, End);

/* Expression nodes */
public sealed record NamedExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right) 
    : ExprNode(Start, End);

public sealed record TestExprNode(uint Start, uint End, ExprNode Left, Token Symbol1, ExprNode Right, Token Symbol2, ExprNode Next) 
    : ExprNode(Start, End);

public sealed record LambdaExprNode(uint Start, uint End, Token Symbol1, StmtNode? Left, Token Symbol2, ExprNode Right, bool IsConditional) 
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

public sealed record UnaryOperatorInvertExprNode(uint Start, uint End, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record BinaryOperatorPowerExprNode(uint Start, uint End, ExprNode Left, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record AtomExprNode(uint Start, uint End, Token? Await, ExprNode Right, ExprNode[] Trailer)
    : ExprNode(Start, End);

public sealed record LiteralNameExprNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralNumberExprNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralStringExprNode(uint Start, uint End, StringToken[] Symbols) : ExprNode(Start, End);

public sealed record LiteralEllipsisExprNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralNoneExprNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralTrueExprNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralFalseExprNode(uint Start, uint End, Token Symbol) : ExprNode(Start, End);

public sealed record LiteralTupleExprNode(uint Start, uint End, Token Symbol1, ExprNode? Right, Token Symbol2)
    : ExprNode(Start, End);

public sealed record LiteralListExprNode(uint Start, uint End, Token Symbol1, ExprNode? Right, Token Symbol2)
    : ExprNode(Start, End);

public sealed record LiteralDictionaryExprNode(uint Start, uint End, Token Symbol1, DictionaryEntryExprNode[] Nodes, Token Symbol2)
    : ExprNode(Start, End);

public sealed record DictionaryEntryExprNode(uint Start, uint End, ExprNode Left, Token? Symbol, ExprNode? Right)
    : ExprNode(Start, End);

public sealed record DictionaryReferenceExprNode(uint Start, uint End, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record LiteralSetExprNode(uint Start, uint End, Token Symbol1, ExprNode[] Nodes, Token Symbol2)
    : ExprNode(Start, End);

public sealed record LiteralSetReferenceExprNode(uint Start, uint End, Token Symbol1, ExprNode Right)
    : ExprNode(Start, End);

public sealed record TestListCompExprNode(uint Start, uint End, ExprNode[] Nodes, Token[] Symbols)
    : ExprNode(Start, End);

public sealed record TrailerDotNameExprNode(uint Start, uint End, Token Symbol, ExprNode Right)
    : ExprNode(Start, End);

public sealed record TrailerCallExprNode(uint Start, uint End, Token Symbol1, ExprNode? Right, Token Symbol2)
    : ExprNode(Start, End);

public sealed record TrailerIndexExprNode(uint Start, uint End, Token Symbol1, ExprNode Right, Token Symbol2)
    : ExprNode(Start, End);

public sealed record SubscriptListExprNode(uint Start, uint End, ExprNode[] Nodes, Token[] Symbols)
    : ExprNode(Start, End);

public sealed record SubscriptExprNode(uint Start, uint End, ExprNode? First, Token? Symbol1, ExprNode? Second, Token? Symbol2, ExprNode? Third)
    : ExprNode(Start, End);

public sealed record ExprListExprNode(uint Start, uint End, ExprNode[] Nodes, Token[] Symbols)
    : ExprNode(Start, End);

public sealed record TestListExprNode(uint Start, uint End, ExprNode[] Nodes, Token[] Symbols)
    : ExprNode(Start, End);

public sealed record ArgListExprNode(uint Start, uint End, ExprNode[] Nodes, Token[] Symbols)
    : ExprNode(Start, End);

public sealed record ArgumentExprNode(uint Start, uint End, ExprNode? Left, Token? Symbol, ExprNode? Right)
    : ExprNode(Start, End);

public sealed record SyncCompForExprNode(uint Start, uint End, Token Symbol1, ExprNode Left, Token Symbol2, ExprNode Right, ExprNode? Next)
    : ExprNode(Start, End);

public sealed record CompForExprNode(uint Start, uint End, Token Symbol, ExprNode Right) : ExprNode(Start, End);

public sealed record CompIfExprNode(uint Start, uint End, Token Symbol, ExprNode Right, ExprNode? Next)
    : ExprNode(Start, End);

public sealed record YieldExprNode(uint Start, uint End, Token Symbol, StmtNode Right) : ExprNode(Start, End);

public sealed record YieldFromExprNode(uint Start, uint End, Token Symbol1, Token Symbol2, ExprNode Right)
    : ExprNode(Start, End);


/* Statement nodes */
public abstract record StmtNode(uint Start, uint End) : SyntaxNode(Start, End);

public sealed record SimpleStmtNode(uint Start, uint End, StmtNode[] Nodes, Token[] Separators, Token Newline) : StmtNode(Start, End);



public sealed record DelStmtNode(uint Start, uint End, Token Symbol, ExprNode Right) : StmtNode(Start, End);
public sealed record PassStmtNode(uint Start, uint End, Token Symbol) : StmtNode(Start, End);
public sealed record BreakStmtNode(uint Start, uint End, Token Symbol) : StmtNode(Start, End);
public sealed record ContinueStmtNode(uint Start, uint End, Token Symbol) : StmtNode(Start, End);
public sealed record ReturnStmtNode(uint Start, uint End, Token Symbol, StmtNode? Right) : StmtNode(Start, End);
public sealed record RaiseStmtNode(uint Start, uint End, Token Symbol1, ExprNode? Left, Token? Symbol2, ExprNode? Right) : StmtNode(Start, End);
public sealed record YieldStmtNode(uint Start, uint End, Token Symbol, StmtNode Right) : StmtNode(Start, End);
public sealed record YieldFromStmtNode(uint Start, uint End, Token Symbol1, Token Symbol2, ExprNode Right) : StmtNode(Start, End);
public sealed record GlobalStmtNode(uint Start, uint End, Token Symbol, ExprNode[] Nodes, Token[] Separators) : StmtNode(Start, End);
public sealed record NonlocalStmtNode(uint Start, uint End, Token Symbol, ExprNode[] Nodes, Token[] Separators) : StmtNode(Start, End);
public sealed record AssertStmtNode(uint Start, uint End, Token Symbol1, ExprNode Left, Token? Symbol2, ExprNode? Right) : StmtNode(Start, End);
public sealed record DottedNameStmtNode(uint Start, uint End, ExprNode[] Nodses, Token[] Separators) : StmtNode(Start, End);
public sealed record DottedAsNamesStmtNode(uint Start, uint End, StmtNode[] Nodes, Token[] Separators) : StmtNode(Start, End);
public sealed record ImportAsNamesStmtNode(uint Start, uint End, StmtNode[] Nodes, Token[] Separators) : StmtNode(Start, End);
public sealed record DottedAsNameStmtNode(uint Start, uint End, StmtNode Left, Token Symbol, ExprNode Right) : StmtNode(Start, End);
public sealed record ImportAsNameStmtNode(uint Start, uint End, ExprNode Left, Token? Symbol, ExprNode? Right) : StmtNode(Start, End);
public sealed record ImportNameStmtNode(uint Start, uint End, Token Symbol, StmtNode Right) : StmtNode(Start, End);
public sealed record FromImportStmtNode(uint Start, uint End, Token Symbol1, Token[] Dots, StmtNode? Left, Token Symbol2, Token? Symbol3, StmtNode? Right, Token? Symbol4) : StmtNode(Start, End);
