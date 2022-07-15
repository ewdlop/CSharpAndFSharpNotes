using LLVMApp.Lexers;
using LLVMApp.Parsers;
using LLVMSharp.Interop;

namespace LLVMApp.Contexts;

public unsafe class LLVMContext : IDisposable
{
    private readonly sbyte* _name;
    private readonly LLVMOpaqueExecutionEngine* _engine;
    private LLVMModuleRef _module;
    private LLVMPassManagerRef _passManager;
    private LLVMBuilderRef _builder;
    private ILexer scanner;
    private IParser parser;
    private readonly sbyte _errorMessage;
    private bool disposedValue;

    public LLVMContext(sbyte* name)
    {
        _name = name;
    }

    public void Start()
    {
        //https://llvm.org/docs/_images/MCJIT-engine-builder.png
        // Make the module, which holds all the code.

        _module = LLVM.ModuleCreateWithName(_name);
        _builder = LLVM.CreateBuilder();

        LLVM.LinkInMCJIT(); //https://llvm.org/docs/MCJITDesignAndImplementation.html
        LLVM.InitializeX86TargetInfo();
        LLVM.InitializeX86Target();
        LLVM.InitializeX86TargetMC();
        fixed (LLVMOpaqueExecutionEngine** enginePtr = &_engine)
        {
            fixed (sbyte* errorMessagePtr = &_errorMessage)
            {
                if (LLVM.CreateExecutionEngineForModule(enginePtr, _module, &errorMessagePtr) == 1)
                {
                    Console.WriteLine(errorMessagePtr->ToString());
                    LLVM.DisposeMessage(errorMessagePtr);
                    return;
                }
            }
            // Create a function pass manager for this engine
            _passManager = LLVM.CreateFunctionPassManagerForModule(_module);

            // Set up the optimizer pipeline.  Start with registering info about how the
            // target lays out data structures.
            // LLVM.DisposeTargetData(LLVM.GetExecutionEngineTargetData(engine));

            // Provide basic AliasAnalysis support for GVN.
            LLVM.AddBasicAliasAnalysisPass(_passManager);

            // Promote allocas to registers.
            LLVM.AddPromoteMemoryToRegisterPass(_passManager);

            // Do simple "peephole" optimizations and bit-twiddling optzns.
            LLVM.AddInstructionCombiningPass(_passManager);

            // Reassociate expressions.
            LLVM.AddReassociatePass(_passManager);

            // Eliminate Common SubExpressions.
            LLVM.AddGVNPass(_passManager);

            // Simplify the control flow graph (deleting unreachable blocks, etc).
            LLVM.AddCFGSimplificationPass(_passManager);

            if(LLVM.InitializeFunctionPassManager(_passManager) == 1)
            {
                
            }
        }
        
        IParserListener codeGenlistener = new CodeGenParserListener(_engine, _passManager, new CodeGenVisitor(_module, _builder));
        Dictionary<char, int> binaryOperatorPrecedence = new()
        {
            ['<'] = 10,
            ['+'] = 20,
            ['-'] = 20,
            ['*'] = 40
        };

        scanner = new Lexer(Console.In, binaryOperatorPrecedence);
        parser = new Parser(scanner, codeGenlistener);

        MainLoop();

        
        // Dispose of the module, pass manager, and builder.
        Dispose(true);
    }

    private void MainLoop()
    {
        // top ::= definition | external | expression | ';'
        while (true)
        {
            Console.Write("ready> ");
            switch (scanner.CurrentToken)
            {
                case (int)Token.EOF:
                    return;
                case ';':
                    scanner.GetNextToken();
                    break;
                case (int)Token.DEF:
                    parser.HandleDefinition();
                    break;
                case (int)Token.EXTERN:
                    parser.HandleExtern();
                    break;
                default:
                    parser.HandleTopLevelExpression();
                    break;
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }
            LLVM.DisposeBuilder(_builder);
            LLVM.DisposeModule(_module);
            LLVM.DisposePassManager(_passManager);
            LLVM.DisposeExecutionEngine(_engine);
            disposedValue = true;
        }
    }

    // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~LLVMContext()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
