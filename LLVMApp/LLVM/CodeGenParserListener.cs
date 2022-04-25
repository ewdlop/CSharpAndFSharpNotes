using LLVMApp.AST;
using LLVMApp.Parsers;

namespace LLVMApp.LLVM;

public class CodeGenParserListener : IParserListener
{
    public void EnterHandleDefinition(FunctionExpressionAST data)
    {
        throw new NotImplementedException();
    }

    public void EnterHandleExtern(PrototypeExpressionAST data)
    {
        throw new NotImplementedException();
    }

    public void EnterHandleTopLevelExpression(FunctionExpressionAST data)
    {
        throw new NotImplementedException();
    }

    public void ExitHandleDefinition(FunctionExpressionAST data)
    {
        throw new NotImplementedException();
    }

    public void ExitHandleExtern(PrototypeExpressionAST data)
    {
        throw new NotImplementedException();
    }

    public void ExitHandleTopLevelExpression(FunctionExpressionAST data)
    {
        throw new NotImplementedException();
    }
}
