
namespace RoslynPythonCoreParser;

public partial class PythonCoreParser
{

    public required PythonCoreTokenizer Lexer;

    private uint FlowLevel = 0;
    
    private StmtNode ParseVarArgsList()
    {
        throw new NotImplementedException();
    }
}