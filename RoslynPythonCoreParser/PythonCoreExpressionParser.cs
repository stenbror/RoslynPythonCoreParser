using System.Diagnostics.SymbolStore;

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
    
    public ExprNode ParseLambdaDef(bool isConditional)
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseOrTest()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseAndTest()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseNotTest()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseComparison()
    {
        throw new NotImplementedException();
    }
    
    public ExprNode ParseStarExpr()
    {
        throw new NotImplementedException();
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