﻿namespace RoslynPythonCoreParser;

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
    
    private StmtNode ParsePassStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseFlowStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseBreakStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseContinueStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseReturnStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseRaiseStmt()
    {
        throw new NotImplementedException();
    }
    
    private StmtNode ParseImportStmt()
    {
        throw new NotImplementedException();
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
    
    private StmtNode ParseGlobalStmt()
    {
        throw new NotImplementedException();
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