using LLVMApp.AST;
using LLVMApp.Parsers;
using LLVMSharp.Interop;
using System.Runtime.InteropServices;

namespace LLVMApp.Contexts;

public unsafe class CodeGenParserListener : IParserListener
{
    private readonly CodeGenVisitor _visitor;

    private readonly LLVMOpaqueExecutionEngine* _executionEngine;

    private readonly LLVMPassManagerRef _passManager;

    public CodeGenParserListener(
        LLVMOpaqueExecutionEngine* executionEngine, 
        LLVMPassManagerRef passManager,
        CodeGenVisitor visitor)
    {
        _visitor = visitor;
        _executionEngine = executionEngine;
        _passManager = passManager;
    }
    


    public void EnterHandleDefinition(FunctionExpressionAST data)
    {

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
        ArgumentNullException.ThrowIfNull(data);
        _visitor.Visit(data);
        LLVMValueRef function = _visitor.PopStack();
        LLVM.DumpValue(function);

        if(LLVM.RunFunctionPassManager(_passManager, function) != 1)
        {
            LLVM.DumpValue(function); // Dump the function for exposition purposes.
        }
        else
        {
            
        }
    }

    public void ExitHandleExtern(PrototypeExpressionAST data)
    {
        _visitor.Visit(data);
        LLVM.DumpValue(_visitor.PopStack());
    }

    public void ExitHandleTopLevelExpression(FunctionExpressionAST data)
    {
        _visitor.Visit(data);
        LLVMValueRef anonymousFunction = _visitor.PopStack();
        LLVM.DumpValue(anonymousFunction);
        delegate* managed<LLVMOpaqueExecutionEngine*, LLVMOpaqueValue*, void*> delgeateFunc = &LLVM.GetPointerToGlobal;
        
        
        delgeateFunc(_executionEngine, anonymousFunction);
        //stuck rip
        //var delgeateFunc = Marshal.GetDelegateForFunctionPointer<Program.Delegate>();
        if(LLVM.RunFunctionPassManager(_passManager, anonymousFunction) != 1);
        {
            LLVM.DumpValue(anonymousFunction);
            //Console.WriteLine($"Evaluated to {delgeateFunc()}" );
        }

    }
}
