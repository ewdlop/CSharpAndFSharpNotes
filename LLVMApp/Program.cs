using LLVMApp.Lexers;
using LLVMApp.LLVM;
using LLVMApp.Parsers;
using LLVMSharp;
using LLVMSharp.Interop;
using System.Text;

Run();
unsafe static void Run()
{
    //https://llvm.org/docs/_images/MCJIT-engine-builder.png
    sbyte[] bytes = (sbyte[])(Array)Encoding.ASCII.GetBytes("Convert");
    fixed(sbyte* ptr = bytes)
    {
        LLVMModuleRef module = LLVM.ModuleCreateWithName(ptr);
        LLVMBuilderRef builder = LLVM.CreateBuilder();

        LLVM.LinkInMCJIT(); //https://llvm.org/docs/MCJITDesignAndImplementation.html
        LLVM.InitializeX86TargetInfo();
        LLVM.InitializeX86Target();
        LLVM.InitializeX86TargetMC();
        LLVMOpaqueExecutionEngine* engine;
        sbyte* errorMessage;
        if (LLVM.CreateExecutionEngineForModule(&engine, module, &errorMessage) == 1)
        {
            Console.WriteLine(errorMessage->ToString());
            // LLVM.DisposeMessage(errorMessage);
            return;
        }
        //IParserListener codeGenlistener = new CodeGenParserListener(engine, passManager, new CodeGenVisitor(module, builder));
        //var binaryOperatorPrecedence = new Dictionary<char, int>
        //{
        //    ['<'] = 10,
        //    ['+'] = 20,
        //    ['-'] = 20,
        //    ['*'] = 40
        //};

        //ILexer scanner = new Lexer(Console.In, binaryOperatorPrecedence);
        //IParser parser = new Parser(scanner, codeGenlistener);

        //MainLoop(scanner,parser);
    }
}


static void MainLoop(ILexer lexer, IParser parser)
{
    // top ::= definition | external | expression | ';'
    while (true)
    {
        Console.Write("ready> ");
        switch (lexer.CurrentToken)
        {
            case (int)Token.EOF:
                return;
            case ';':
                lexer.GetNextToken();
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