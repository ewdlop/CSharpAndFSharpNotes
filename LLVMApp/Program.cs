using LLVMApp.Lexers;
using LLVMApp.Parsers;

Run();
unsafe static void Run()
{
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