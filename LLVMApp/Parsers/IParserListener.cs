using LLVMApp.AST;

namespace LLVMApp.Parsers;

public interface IParserListener
{
    void EnterHandleDefinition(FunctionExpressionAST data);

    void ExitHandleDefinition(FunctionExpressionAST data);

    void EnterHandleExtern(PrototypeExpressionAST data);

    void ExitHandleExtern(PrototypeExpressionAST data);

    void EnterHandleTopLevelExpression(FunctionExpressionAST data);

    void ExitHandleTopLevelExpression(FunctionExpressionAST data);
}