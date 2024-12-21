using System.Runtime.CompilerServices;

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
    
    private StmtNode ParseExprStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseTestListStarExpr()
    {
        throw new NotImplementedException();
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
    
    private StmtNode ParseImportNameStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseImportFromStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseImportAsNameStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseDottedAsNameStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseImportAsNamesStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseDottedAsNamesStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseDottedNameStmt()
    {
        throw new NotImplementedException();
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
    
    private StmtNode ParseNonlocalStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseAssertStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseCompoundStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseIfStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseWhileStmt()
    {
        throw new NotImplementedException();
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