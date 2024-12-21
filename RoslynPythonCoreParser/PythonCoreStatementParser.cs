
namespace RoslynPythonCoreParser;

public partial class PythonCoreParser
{
    /// <summary>
    ///  Handle grammar rule for simple_stmt | compound_stmt
    /// </summary>
    /// <returns> StmtNode </returns>
    private StmtNode ParseStmt()
    {
        return Lexer.Symbol switch
        {
            IfToken or WhileToken or AsyncToken or ForToken or TryToken or WithToken or DefToken or ClassToken or  BinaryOperatorMatricesToken =>
                ParseCompoundStmt(),
            NameToken( _ , _ , "match" , _ ) => ParseCompoundStmt(), /* Contextual keyword 'match' */
            _ => ParseSimpleStmt()
        };
    }
    
    /// <summary>
    ///  Handle grammar rule: small_stmt (';' small_stmt)* [';'] NEWLINE
    /// </summary>
    /// <returns> StmtNode </returns>
    private StmtNode ParseSimpleStmt()
    {
        var pos = Lexer.Position;
        var nodes = new List<StmtNode>();
        var separators = new List<Token>();
        
        nodes.Add(ParseSmallStmt());

        while (Lexer.Symbol is SemiColonToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();

            if (Lexer.Symbol is NewlineToken) break;
                
            nodes.Add(ParseSmallStmt());
        }

        if (Lexer.Symbol is not NewlineToken) throw new Exception();
        
        var newline = Lexer.Symbol;
        Lexer.Advance();

        return new SimpleStmtNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray(), newline);
    }
    
    /// <summary>
    ///  Handle grammar rule: (expr_stmt | del_stmt | pass_stmt | flow_stmt |
    /// import_stmt | global_stmt | nonlocal_stmt | assert_stmt)
    /// </summary>
    /// <returns> StmtNode </returns>
    private StmtNode ParseSmallStmt()
    {
        return Lexer.Symbol switch
        {
            DelToken => ParseDelStmt(),
            PassToken => ParsePassStmt(),
            BreakToken or ContinueToken or ReturnToken or RaiseToken or YieldToken => ParseFlowStmt(),
            ImportToken or FromToken => ParseImportStmt(),
            GlobalToken => ParseGlobalStmt(),
            NonlocalToken => ParseNonlocalStmt(),
            AssertToken => ParseAssertStmt(),
            _ => ParseExprStmt()
        };
    }
    
    /// <summary>
    ///  Handle grammar rule:  testlist_star_expr (annassign | augassign (yield_expr|testlist) |
    ///[('=' (yield_expr|testlist_star_expr))+ [TYPE_COMMENT]] )
    /// annassign: ':' test ['=' (yield_expr|testlist_star_expr)]
    /// </summary>
    /// <returns> AnnAssignStmtNode | AssignmentStmtNode | PlusAssignStmtNode | MinusAssignStmtNode |
    /// MulAssignStmtNode | MatricesAssignStmtNode | DivAssignStmtNode | ModuloAssignStmtNode |
    /// BitAndAssignStmtNode | BitOrAssignStmtNode | BitXorAssignStmtNode | ShiftLeftAssignStmtNode |
    /// ShiftRightAssignStmtNode | PowerAssignStmtNode | FloorDivAssignStmtNode | StmtNode </returns>
    /// <exception cref="Exception"></exception>
    private StmtNode ParseExprStmt()
    {
        var pos = Lexer.Position;
        
        var left = ParseTestListStarExpr();

        switch (Lexer.Symbol)
        {
            case ColonToken:
            {
                var symbol1 = Lexer.Symbol;
                Lexer.Advance();
                
                var right = ParseTest();

                if (Lexer.Symbol is AssignToken)
                {
                    var symbol2 = Lexer.Symbol;
                    Lexer.Advance();
                    
                    var next = Lexer.Symbol is YieldToken ? ParseYieldStmt() : ParseTestListStarExpr();

                    return new AnnAssignStmtNode(pos, Lexer.Position, left, symbol1, right, symbol2, next);
                }
                
                return new AnnAssignStmtNode(pos, Lexer.Position, left, symbol1, right, null, null);
            }
            case AssignToken:
            {
                var nodes = new List<StmtNode>();

                var symbol = Lexer.Symbol;
                Lexer.Advance();
                
                var right = Lexer.Symbol is YieldToken ? ParseYieldStmt() : ParseTestListStarExpr();
                nodes.Add(new AssignmentElementStmtNode(pos, Lexer.Position, symbol, right));

                while (Lexer.Symbol is AssignToken)
                {
                    symbol = Lexer.Symbol;
                    Lexer.Advance();
                    
                    right = Lexer.Symbol is YieldToken ? ParseYieldStmt() : ParseTestListStarExpr();
                    nodes.Add(new AssignmentElementStmtNode(pos, Lexer.Position, symbol, right));
                }

                if (Lexer.Symbol is TypeCommentToken)
                {
                    symbol = Lexer.Symbol;
                    Lexer.Advance();
                }

                return new AssignmentStmtNode(pos, Lexer.Position, left, nodes.ToArray(),
                    symbol is TypeCommentToken ? symbol : null);
            }
            case PlusAssignToken:
            case MinusAssignToken:
            case MulAssignToken:
            case MatricesAssignToken:
            case DivAssignToken:
            case ModuloAssignToken:
            case BitAndAssignToken:
            case BitOrAssignToken:
            case BitXorAssignToken:
            case ShiftLeftAssignToken:
            case ShiftRightAssignToken:
            case PowerAssignToken:
            case FloorDivAssignToken:
            {
                var symbol = Lexer.Symbol;
                Lexer.Advance();

                var right = Lexer.Symbol is YieldToken ? ParseYieldExpr() : ParseTestList();

                return symbol switch
                {
                    PlusAssignToken => new PlusAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    MinusAssignToken => new MinusAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    MulAssignToken => new MulAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    MatricesAssignToken => new MatricesAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    DivAssignToken => new DivAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    ModuloAssignToken => new ModuloAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    BitAndAssignToken => new BitAndAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    BitOrAssignToken => new BitOrAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    BitXorAssignToken => new BitXorAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    ShiftLeftAssignToken => new ShiftLeftAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    ShiftRightAssignToken => new ShiftRightAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    PowerAssignToken => new PowerAssignStmtNode(pos, Lexer.Position, left, symbol, right),
                    _ => new FloorDivAssignStmtNode(pos, Lexer.Position, left, symbol, right)
                };
            }
                
            default:
                return left;
        }
    }
    
    /// <summary>
    ///  Handle grammar rule: (test|star_expr) (',' (test|star_expr))* [',']
    /// </summary>
    /// <returns> TestListStarExprStmtNode </returns>
    private StmtNode ParseTestListStarExpr()
    {
        var pos = Lexer.Position;
        var nodes = new List<ExprNode>();
        var separators = new List<Token>();
        
        nodes.Add( Lexer.Symbol is BinaryOperatorMulToken ? ParseStarExpr() : ParseTest() );

        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();

            if (Lexer.Symbol is PlusAssignToken 
                or MinusAssignToken 
                or MulAssignToken 
                or MatricesAssignToken
                or DivAssignToken
                or ModuloAssignToken
                or BitAndAssignToken
                or BitOrAssignToken
                or BitXorAssignToken
                or ShiftLeftAssignToken
                or ShiftRightAssignToken
                or PowerAssignToken
                or FloorDivAssignToken
                or SemiColonToken
                or NewlineToken
                or AssignToken
                or ColonToken) break;
            
            nodes.Add( Lexer.Symbol is BinaryOperatorMulToken ? ParseStarExpr() : ParseTest() );
        }

        return new TestListStarExprStmtNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray());
    }
    
    /// <summary>
    ///  Handle Grammar rule: 'del' expr_list
    /// </summary>
    /// <returns> DelStmtNode </returns>
    private StmtNode ParseDelStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseExprList();

        return new DelStmtNode(pos, Lexer.Position, symbol, right);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'pass'
    /// </summary>
    /// <returns> PassStmtNode </returns>
    private StmtNode ParsePassStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();
        
        return new PassStmtNode(pos, Lexer.Position, symbol);
    }
    
    /// <summary>
    ///  Handle grammar rule: break_stmt | continue_stmt | return_stmt | raise_stmt | yield_stmt
    /// </summary>
    /// <returns> StmtNode </returns>
    private StmtNode ParseFlowStmt()
    {
        if (FlowLevel == 0) throw new Exception();
        
        return Lexer.Symbol switch
        {
            BreakToken => ParseBreakStmt(),
            ContinueToken => ParseContinueStmt(),
            ReturnToken => ParseReturnStmt(),
            RaiseToken => ParseRaiseStmt(),
            _ => ParseYieldStmt()
        };
    }
    
    /// <summary>
    ///  Handle grammar rule: 'break'
    /// </summary>
    /// <returns> BreakStmtNode </returns>
    private StmtNode ParseBreakStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();
        
        return new BreakStmtNode(pos, Lexer.Position, symbol);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'continue'
    /// </summary>
    /// <returns> ContinueStmtNode </returns>
    private StmtNode ParseContinueStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();
        
        return new ContinueStmtNode(pos, Lexer.Position, symbol);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'return' [testlist_star_expr]
    /// </summary>
    /// <returns> ReturnStmtNode </returns>
    private StmtNode ParseReturnStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = Lexer.Symbol is NewlineToken or SemiColonToken ? null : ParseTestListStarExpr();

        return new ReturnStmtNode(pos, Lexer.Position, symbol, right);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'raise' [test ['from' test]]
    /// </summary>
    /// <returns> RaiseStmtNode </returns>
    private StmtNode ParseRaiseStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        if (Lexer.Symbol is NewlineToken or SemiColonToken) return new RaiseStmtNode(pos, Lexer.Position, symbol, null, null, null);

        var left = ParseTest();

        if (Lexer.Symbol is FromToken)
        {
            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseTest();

            return new RaiseStmtNode(pos, Lexer.Position, symbol, left, symbol2, right);
        }
        
        return new RaiseStmtNode(pos, Lexer.Position, symbol, left, null, null);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'yield' ( ( 'from' Test ) | TestListStarExpr ) 
    /// </summary>
    /// <returns> YieldFromStmtNode | YieldStmtNode </returns>
    private StmtNode ParseYieldStmt()
    {
        var pos = Lexer.Position;
        var symbol1 = Lexer.Symbol;
        Lexer.Advance();

        if (Lexer.Symbol is FromToken)
        {
            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseTest();

            return new YieldFromStmtNode(pos, Lexer.Position, symbol1, symbol2, right);
        }

        var right2 = ParseTestListStarExpr();

        return new YieldStmtNode(pos, Lexer.Position, symbol1, right2);
    }
    
    /// <summary>
    ///  Handle grammar rule: import_stmt | import_from_stmt
    /// </summary>
    /// <returns> StmtNode </returns>
    private StmtNode ParseImportStmt()
    {
        return Lexer.Symbol switch
        {
            ImportToken => ParseImportStmt(),
            _ => ParseImportFromStmt()
        };
    }
    
    /// <summary>
    ///  Handle grammar rule: 'import' dotted_as_names 
    /// </summary>
    /// <returns> ImportNameStmtNode </returns>
    private StmtNode ParseImportNameStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseDottedAsNamesStmt();

        return new ImportNameStmtNode(pos, Lexer.Position, symbol, right);
    }
    
    /// <summary>
    ///  Handle grammar rule: ('from' (('.' | '...')* dotted_name | ('.' | '...')+)
    /// 'import' ('*' | '(' import_as_names ')' | import_as_names))
    /// </summary>
    /// <returns> FromImportStmtNode </returns>
    /// <exception cref="SyntaxError"></exception>
    private StmtNode ParseImportFromStmt()
    {
        var pos = Lexer.Position;
        var symbol1 = Lexer.Symbol; /* 'from' */
        Lexer.Advance();

        var dots = new List<Token>();

        while (Lexer.Symbol is DotToken or ElipsisToken)
        {
            dots.Add(Lexer.Symbol);
            Lexer.Advance();
        }

        if (Lexer.Symbol is ImportToken && dots.Count == 0) throw new SyntaxError(Lexer.Position, "Missing 'from' elements in import statement");

        var left = Lexer.Symbol is not ImportToken ? ParseDottedNameStmt() : null;

        if (Lexer.Symbol is not ImportToken) throw new SyntaxError(Lexer.Position, "Missing 'import' in from import statement");
        var symbol2 = Lexer.Symbol; /* 'import' */
        Lexer.Advance();

        switch (Lexer.Symbol)
        {
            case BinaryOperatorMulToken:
            {
                var symbol3 = Lexer.Symbol; /* '*' */
                Lexer.Advance();
                
                return new FromImportStmtNode(pos, Lexer.Position, symbol1, dots.ToArray(), left, symbol2, symbol3, null, null);
            }
            case LeftParenToken:
            {
                var symbol3 = Lexer.Symbol;
                Lexer.Advance();

                var right = ParseImportAsNamesStmt();

                if (Lexer.Symbol is not RightParenToken) throw new SyntaxError(Lexer.Position, "");
                var symbol4 = Lexer.Symbol;
                Lexer.Advance();
                
                return new FromImportStmtNode(pos, Lexer.Position, symbol1, dots.ToArray(), left, symbol2, symbol3, right, symbol4);
            }
            default:
            {
                var right = ParseImportAsNamesStmt();

                return new FromImportStmtNode(pos, Lexer.Position, symbol1, dots.ToArray(), left, symbol2, null, right, null);
            }
        }
    }
    
    /// <summary>
    ///  Handle grammar rule: NAME [ 'as' NAME ]
    /// </summary>
    /// <returns> ImportAsNameStmtNode </returns>
    /// <exception cref="SyntaxError"></exception>
    private StmtNode ParseImportAsNameStmt()
    {
        var pos = Lexer.Position;
        
        if (Lexer.Symbol is not NameToken) throw new SyntaxError(Lexer.Position, "Missing NAME literal in import statement");
        var left = ParseAtom();

        if (Lexer.Symbol is AsToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();
            
            if (Lexer.Symbol is not NameToken) throw new SyntaxError(Lexer.Position, "Missing NAME literal after 'as' in import statement");
            var right = ParseAtom();

            return new ImportAsNameStmtNode(pos, Lexer.Position, left, symbol, right);
        }

        return new ImportAsNameStmtNode(pos, Lexer.Position, left, null, null);
    }
    
    /// <summary>
    ///  Handling grammar rule: dotted_name ['as' NAME]
    /// </summary>
    /// <returns> DottedAsNameStmtNode | DottedNameStmtNode </returns>
    /// <exception cref="SyntaxError"></exception>
    private StmtNode ParseDottedAsNameStmt()
    {
        var pos = Lexer.Position;
        var left = ParseDottedNameStmt();

        if (Lexer.Symbol is AsToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();
            
            if (Lexer.Symbol is not NameToken) throw new SyntaxError(Lexer.Position, "Missing NAME literal after 'as' in import statement");
            var right = ParseAtom();

            return new DottedAsNameStmtNode(pos, Lexer.Position, left, symbol, right);
        }

        return left;
    }
    
    /// <summary>
    ///  Handle grammar rule: import_as_name (',' import_as_name)* [',']
    /// </summary>
    /// <returns> ImportAsNamesStmtNode | ImportAsNameStmtNode </returns>
    private StmtNode ParseImportAsNamesStmt()
    {
        var pos = Lexer.Position;
        var nodes = new List<StmtNode>();
        var separators = new List<Token>();
        
        nodes.Add(ParseImportAsNameStmt());

        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();

            if (Lexer.Symbol is NewlineToken or SemiColonToken) break;
            
            nodes.Add(ParseImportAsNameStmt());
        }
        
        return nodes.Count == 1
            ? nodes[0]
            : new ImportAsNamesStmtNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray());
    }
    
    /// <summary>
    ///  Handle grammar rule: dotted_as_name (',' dotted_as_name)*
    /// </summary>
    /// <returns> DottedAsNamesStmtNode | DottedNameStmtNode </returns>
    private StmtNode ParseDottedAsNamesStmt()
    {
        var pos = Lexer.Position;
        var nodes = new List<StmtNode>();
        var separators = new List<Token>();
        
        nodes.Add(ParseDottedAsNameStmt());

        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();
            
            nodes.Add(ParseDottedAsNameStmt());
        }

        return nodes.Count == 1
            ? nodes[0]
            : new DottedAsNamesStmtNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray());
    }
    
    /// <summary>
    ///  Handle grammar rule: NAME ('.' NAME)*
    /// </summary>
    /// <returns> DottedNameStmtNode </returns>
    private StmtNode ParseDottedNameStmt()
    {
        var nodes = new List<ExprNode>();
        var separators = new List<Token>();
        var pos = Lexer.Position;
        
        if (Lexer.Symbol is not NameToken) throw new SyntaxError(Lexer.Position, "Missing NAME literal in named import statement");
        nodes.Add(ParseAtom());
        
        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();
            
            if (Lexer.Symbol is not NameToken) throw new SyntaxError(Lexer.Position, "Missing NAME literal in named import statement after ','");
            nodes.Add(ParseAtom());
        }

        return new DottedNameStmtNode(pos, Lexer.Position, nodes.ToArray(), separators.ToArray());
    }
    
    /// <summary>
    ///  Handle grammar rule: 'global' NAME (',' NAME)*
    /// </summary>
    /// <returns> GlobalStmtNode </returns>
    private StmtNode ParseGlobalStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var nodes = new List<ExprNode>();
        var separators = new List<Token>();

        if (Lexer.Symbol is not NameToken) throw new SyntaxError(Lexer.Position, "Missing NAME literal in 'global' statement");
        nodes.Add(ParseAtom());

        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();
            
            if (Lexer.Symbol is not NameToken) throw new SyntaxError(Lexer.Position, "Missing NAME literal in 'global' statement after ','");
            nodes.Add(ParseAtom());
        }

        return new GlobalStmtNode(pos, Lexer.Position, symbol, nodes.ToArray(), separators.ToArray());
    }
    
    /// <summary>
    ///  Handle grammar rule: 'nonlocal' NAME (',' NAME)*
    /// </summary>
    /// <returns> NonlocalStmtNode </returns>
    private StmtNode ParseNonlocalStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var nodes = new List<ExprNode>();
        var separators = new List<Token>();

        if (Lexer.Symbol is not NameToken) throw new SyntaxError(Lexer.Position, "Missing NAME literal in 'nonlocal' statement");
        nodes.Add(ParseAtom());

        while (Lexer.Symbol is CommaToken)
        {
            separators.Add(Lexer.Symbol);
            Lexer.Advance();
            
            if (Lexer.Symbol is not NameToken) throw new SyntaxError(Lexer.Position, "Missing NAME literal in 'nonlocal' statement after ','");
            nodes.Add(ParseAtom());
        }

        return new NonlocalStmtNode(pos, Lexer.Position, symbol, nodes.ToArray(), separators.ToArray());
    }
    
    /// <summary>
    ///  Grammar rule for: 'assert' test [ ',' test ]
    /// </summary>
    /// <returns> AssertStmtNode </returns>
    private StmtNode ParseAssertStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var left = ParseTest();

        if (Lexer.Symbol is CommaToken)
        {
            var symbol2 = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseTest();
            
            return new AssertStmtNode(pos, Lexer.Position, symbol, left, symbol2, right);
        }

        return new AssertStmtNode(pos, Lexer.Position, symbol, left, null, null);
    }
    
    /// <summary>
    ///  Handle grammar rule: if_stmt | while_stmt | for_stmt | try_stmt | with_stmt | funcdef | classdef | decorated | async_stmt
    /// </summary>
    /// <returns> StmtNode </returns>
    private StmtNode ParseCompoundStmt()
    {
        return Lexer.Symbol switch
        {
            IfToken => ParseIfStmt(),
            WhileToken => ParseWhileStmt(),
            ForToken => ParseForStmt(),
            TryToken => ParseTryStmt(),
            WithToken => ParseWithStmt(),
            DefToken => ParseFuncDef(),
            ClassToken => ParseClass(),
            AsyncToken => ParseAsyncStmt(),
            NameToken( _ , _ , "match" , _ ) => ParseMatchStmt(),
            _ => ParseDecorated()
        };
    }

    /// <summary>
    ///  Handle grammar rule: 'async' (funcdef | with_stmt | for_stmt)
    /// </summary>
    /// <returns> SyncStmtNode </returns>
    /// <exception cref="SyntaxError"></exception>
    private StmtNode ParseAsyncStmt()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = Lexer.Symbol switch
        {
            DefToken => ParseFuncDef(),
            ForToken => ParseForStmt(),
            WithToken => ParseWithStmt(),
            _ => throw new SyntaxError(Lexer.Position, "Expecting 'def', 'for' or 'with' after 'async' in statement")
        };

        return new AsyncStmtNode(pos, Lexer.Position, symbol, right);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'if' namedexpr_test ':' suite ('elif' namedexpr_test ':' suite)* ['else' ':' suite]
    /// </summary>
    /// <returns> IfStmtNode </returns>
    /// <exception cref="SyntaxError"></exception>
    private StmtNode ParseIfStmt()
    {
        var pos = Lexer.Position;
        var nodes = new List<StmtNode>();
        StmtNode? elsepart = null;
        
        var symbol1 = Lexer.Symbol; /* 'if' */
        Lexer.Advance();

        var left = ParseNamedExpr();
        
        if (Lexer.Symbol is not ColonToken) throw new SyntaxError(Lexer.Position, "Expected ':' in 'if' statement");
        var symbol2 = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseSuiteStmt();

        while (Lexer.Symbol is ElifToken) /* 'elif' */
        {
            nodes.Add(ParseElifStmt());
        }

        if (Lexer.Symbol is ElseToken) /* 'else' */
        {
            elsepart = ParseElseStmt();
        }

        return new IfStmtNode(pos, Lexer.Position, symbol1, left, symbol2, right, nodes.ToArray(), elsepart);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'elif' namedexpr ':' suite
    /// </summary>
    /// <returns> ElifStmtNode </returns>
    /// <exception cref="SyntaxError"></exception>
    private StmtNode ParseElifStmt()
    {
        var pos = Lexer.Position;
        var symbol1 = Lexer.Symbol;
        Lexer.Advance();

        var left = ParseNamedExpr();

        if (Lexer.Symbol is not ColonToken) throw new SyntaxError(Lexer.Position, "Expected ':' in 'elif' statement");
        var symbol2 = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseSuiteStmt();

        return new ElifStmtNode(pos, Lexer.Position, symbol1, left, symbol2, right);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'else' ':' Suite
    /// </summary>
    /// <returns> ElseStmtNode </returns>
    /// <exception cref="SyntaxError"></exception>
    private StmtNode ParseElseStmt()
    {
        var pos = Lexer.Position;
        var symbol1 = Lexer.Symbol;
        Lexer.Advance();

        if (Lexer.Symbol is not ColonToken) throw new SyntaxError(Lexer.Position, "Expecting ':' in 'else' statement");
        var symbol2 = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseSuiteStmt();

        return new ElseStmtNode(pos, Lexer.Position, symbol1, symbol2, right);
    }
    
    /// <summary>
    ///  Handle grammar rule: 'while' namedexpr_test ':' suite ['else' ':' suite]
    /// </summary>
    /// <returns> WhileStmtNode </returns>
    /// <exception cref="SyntaxError"></exception>
    private StmtNode ParseWhileStmt()
    {
        var pos = Lexer.Position;
        var symbol1 = Lexer.Symbol;
        Lexer.Advance();

        var left = ParseNamedExpr();
        
        if (Lexer.Symbol is not ColonToken) throw new SyntaxError(Lexer.Position, "Expecting ':' in 'while' statement");
        var symbol2 = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseSuiteStmt();

        var else_part = Lexer.Symbol is ElseToken ? ParseElseStmt() : null;

        return new WhileStmtNode(pos, Lexer.Position, symbol1, left, symbol2, right, else_part);
    }
    
    private StmtNode ParseForStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseTryStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseWithStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseWithItemStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseExceptClauseStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseSuiteStmt()
    {
        throw new NotImplementedException();
    }
}