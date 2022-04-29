using LLVMSharp.Interop;
using System.Text;

namespace LLVMApp.Contexts;

public unsafe record LLVMContext(string Name)
{
    private LLVMOpaqueExecutionEngine* _engine;
    private sbyte _errorMessage;

    public void Start()
    {
        //https://llvm.org/docs/_images/MCJIT-engine-builder.png
        sbyte[] sbytes = (sbyte[])(Array)Encoding.ASCII.GetBytes(Name);
        fixed (sbyte* nameSBytesPtr = sbytes)
        {
            // Make the module, which holds all the code.
            LLVMModuleRef module = LLVM.ModuleCreateWithName(nameSBytesPtr);
            LLVMBuilderRef builder = LLVM.CreateBuilder();

            LLVM.LinkInMCJIT(); //https://llvm.org/docs/MCJITDesignAndImplementation.html
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetMC();
            fixed (LLVMOpaqueExecutionEngine** enginePtr = &_engine)
            {
                fixed (sbyte* errorMessagePtr = &_errorMessage)
                {
                    if (LLVM.CreateExecutionEngineForModule(enginePtr, module, &errorMessagePtr) == 1)
                    {
                        Console.WriteLine(errorMessagePtr->ToString());
                        // LLVM.DisposeMessage(errorMessage);
                        return;
                    }
                    // Create a function pass manager for this engine
                    LLVMPassManagerRef passManager = LLVM.CreateFunctionPassManagerForModule(module);

                    // Set up the optimizer pipeline.  Start with registering info about how the
                    // target lays out data structures.
                    // LLVM.DisposeTargetData(LLVM.GetExecutionEngineTargetData(engine));

                    // Provide basic AliasAnalysis support for GVN.
                    LLVM.AddBasicAliasAnalysisPass(passManager);

                    // Promote allocas to registers.
                    LLVM.AddPromoteMemoryToRegisterPass(passManager);

                    // Do simple "peephole" optimizations and bit-twiddling optzns.
                    LLVM.AddInstructionCombiningPass(passManager);

                    // Reassociate expressions.
                    LLVM.AddReassociatePass(passManager);

                    // Eliminate Common SubExpressions.
                    LLVM.AddGVNPass(passManager);

                    // Simplify the control flow graph (deleting unreachable blocks, etc).
                    LLVM.AddCFGSimplificationPass(passManager);

                    LLVM.InitializeFunctionPassManager(passManager);
                }
            }

        }
    }
}
