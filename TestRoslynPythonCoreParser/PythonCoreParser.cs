namespace TestRoslynPythonCoreParser;

using RoslynPythonCoreParser;

public class PythonCoreParser
{
    [Fact]
    public void TestSyntaxNodeInheritance()
    {
        var node = new BinaryOperatorMinusExprNode(
            0, 3,
                new LiteralNumberExprNode(0, 1, new NumberToken(0, 1, "8", [])),
                new BinaryOperatorPlusToken(1, 2, []),
                new LiteralNumberExprNode(2, 3, new NumberToken(2, 3, "8", []))
            );
        
        Assert.True(node is ExprNode);
    }
}