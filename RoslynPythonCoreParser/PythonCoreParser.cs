
namespace RoslynPythonCoreParser;

public partial class PythonCoreParser
{

    public required PythonCoreTokenizer Lexer;

    private uint FlowLevel = 0;
    
    private StmtNode ParseVarArgsList()
    {
        throw new NotImplementedException();
    }


    private StmtNode ParseDecorated()
    {
        throw new Exception();
    }

    private StmtNode ParseFuncDef()
    {
        throw new Exception();
    }

    private StmtNode ParseClass()
    {
        throw new Exception();
    }
}