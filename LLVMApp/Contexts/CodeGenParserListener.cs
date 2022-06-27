using LLVMApp.AST;
using LLVMApp.Parsers;
using LLVMSharp.Interop;

namespace LLVMApp.Contexts;

public unsafe class CodeGenParserListener : IParserListener
{
    private readonly CodeGenVisitor _visitor;

    private readonly LLVMOpaqueExecutionEngine* _ee;

    private readonly LLVMPassManagerRef _passManager;

    public CodeGenParserListener(
        LLVMOpaqueExecutionEngine* ee, 
        LLVMPassManagerRef passManager,
        CodeGenVisitor visitor)
    {
        _visitor = visitor;
        _ee = ee;
        _passManager = passManager;
    }
    


    public void EnterHandleDefinition(FunctionExpressionAST data)
    {
        ArgumentNullException.ThrowIfNull(data);        
        _visitor.Visit(data);
        LLVMValueRef function = _visitor.Pop();
        LLVM.DumpValue(function);

        LLVM.RunFunctionPassManager(_passManager, function);
        LLVM.DumpValue(function); // Dump the function for exposition purposes.
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
