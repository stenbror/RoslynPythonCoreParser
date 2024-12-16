using System.Diagnostics.SymbolStore;
using System.Net.Sockets;

namespace RoslynPythonCoreParser;

public partial class PythonCoreParser
{
    /// <summary>
    ///  Handling grammar rule: test [ ':=' test ]
    /// </summary>
    /// <returns> NamedExprNode | ExprNode </returns>
    public ExprNode ParseNamedExpr()
    {
        var pos = Lexer.Position;
        var left = ParseExpr();

        if (Lexer.Symbol is not ColonAssignToken) return left;
        
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseExpr();

        return new NamedExprNode(pos, Lexer.Position, left, symbol, right);
    }

    /// <summary>
    ///  Handling grammar rule: or_test [ 'if' or_test 'else' test ] | LambdaDef
    /// </summary>
    /// <returns> TestExprNode | ExprNode </returns>
    /// <exception cref="Exception"></exception>
    public ExprNode ParseTest()
    {
        if (Lexer.Symbol is LambdaToken) return ParseLambdaDef(true);

        var pos = Lexer.Position;
        var left = ParseOrTest();

        if (Lexer.Symbol is not IfToken) return left;

        var symbol1 = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseOrTest();

        if (Lexer.Symbol is not ElseToken) throw new Exception();

        var symbol2 = Lexer.Symbol;
        Lexer.Advance();
        
        var next = ParseTest();

        return new TestExprNode(pos, Lexer.Position, left, symbol1, right, symbol2, next);
    }
    
    private ExprNode ParseLambdaDef(bool isConditional)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    ///  Grammar rule: AndTest ( 'or' AndTest )*
    /// </summary>
    /// <returns> OrTestExprNode | ExprNode </returns>
    private ExprNode ParseOrTest()
    {
        var pos = Lexer.Position;
        var left = ParseAndTest();

        while (Lexer.Symbol is OrToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseAndTest();

            left = new OrTestExprNode(pos, Lexer.Position, left, symbol, right);
        }

        return left;
    }
    
    /// <summary>
    ///  Grammar rule: NotTest ( 'and' NotTest )*
    /// </summary>
    /// <returns> AndTestExprNode | ExprNode </returns>
    private ExprNode ParseAndTest()
    {
        var pos = Lexer.Position;
        var left = ParseNotTest();

        while (Lexer.Symbol is OrToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();

            var right = ParseNotTest();

            left = new AndTestExprNode(pos, Lexer.Position, left, symbol, right);
        }

        return left;
    }
    
    /// <summary>
    ///  Grammar rule: Comparison | 'not' NotTest
    /// </summary>
    /// <returns> NotTestExprNode | ExprNode </returns>
    private ExprNode ParseNotTest()
    {
        if (Lexer.Symbol is not NotToken) return ParseComparison();
        
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseNotTest();

        return new NotTestExprNode(pos, Lexer.Position, symbol, right);
    }
    
    /// <summary>
    ///  Grammar rule:  expr ( ('<' | '<=' | '>' | '>=' | '==' | '!=' | '<>' | 'in' | 'is' | 'is 'not' | 'not' 'in') expr )*
    /// </summary>
    /// <returns> CompareLessExprNode | CompareLessEqualExprNode | CompareEqualExprNode | CompareNotInExprNode |
    /// CompareGreaterExprNode | CompareGreaterEqualExprNode | CompareInExprNode | CompareNotInExprNode |
    /// CompareIsExprNode | CompareIsNotExprNode | ExprNode </returns>
    /// <exception cref="Exception"></exception>
    private ExprNode ParseComparison()
    {
        var pos = Lexer.Position;
        var left = ParseExpr();

        while (Lexer.Symbol is CompareLessToken 
               or CompareLessEqualToken 
               or CompareGreaterToken 
               or CompareLessEqualToken
               or CompareEqualToken
               or CompareNotEqualToken
               or InToken
               or IsToken
               or NotToken)
        {
            var symbol = Lexer.Symbol;
            Lexer.Advance();
            
            if (symbol is IsToken)
            {
                if (Lexer.Symbol is NotToken)
                {
                    var symbol2 = Lexer.Symbol;
                    Lexer.Advance();

                    var right = ParseExpr();

                    left = new CompareIsNotExprNode(pos, Lexer.Position, left, symbol, symbol2, right);
                }
                else
                {
                    var right = ParseExpr();
                    left = new CompareIsExprNode(pos, Lexer.Position, left, symbol, right);
                }
            }
            else if (symbol is NotToken)
            {
                if (Lexer.Symbol is not InToken) throw new Exception();

                var symbol2 = Lexer.Symbol;
                Lexer.Advance();
                var right = ParseExpr();

                left = new CompareNotInExprNode(pos, Lexer.Position, left, symbol, symbol2, right);
            }
            else
            {
                var right = ParseExpr();

                left = symbol switch
                {
                    CompareLessToken => new CompareLessExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareLessEqualToken => new CompareLessEqualExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareEqualToken => new CompareEqualExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareGreaterEqualToken => new CompareGreaterEqualExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareGreaterToken => new CompareGreaterExprNode(pos, Lexer.Position, left, symbol, right),
                    CompareNotEqualToken => new CompareNotEqualExprNode(pos, Lexer.Position, left, symbol, right),
                    InToken => new CompareInExprNode(pos, Lexer.Position, left, symbol, right),
                    _ => throw new Exception()
                };
            }
        }

        return left;
    }
    
    /// <summary>
    ///  Grammar rule: '*' Expr
    /// </summary>
    /// <returns> StarExprNode </returns>
    private ExprNode ParseStarExpr()
    {
        var pos = Lexer.Position;
        var symbol = Lexer.Symbol;
        Lexer.Advance();

        var right = ParseExpr();

        return new StarExprNode(pos, Lexer.Position, symbol, right);
    }
    
    public ExprNode ParseExpr()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseXorExpr()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseAndExpr()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseShiftExpr()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseArithExpr()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseTerm()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseFactor()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParsePower()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseAtomExpr()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseAtom()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseTestListComp()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseTrailer()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseSubscriptList()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseExprList()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseTestList()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseDictorSetMaker()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseArgList()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseArgument()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseCompIter()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseSyncCompFor()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseCompFor()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseCompIf()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseYieldExpr()
    {
        throw new NotImplementedException();
    }
}