using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behaviors;
using CSharpClassLibrary.MiniComplierFrontEnd.Parsers;
using CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behaviors;

//string test = "{ int i;\nint j;\nwhile( true ) { i = i + 1; } }";
//string test = "{ int i; i = 2;}";
string test = "{ int i; int j; float v; float x; float[100] a;\n" +
    "while(true){\n" +
    "do i = i+1; while( a[i] < v);\n" +
    "do j = j-1; while( a[j] > v);\n" +
    "if( i >= 1) break;\n" +
    "x=a[i];a[i]=a[j];a[j]=x;\n" +
    "\n}" +
    "}";
string test2 = "{ float[100] a;\n" +
    "while(true){\n" +
    "a[50] = 5;\n" +
    "}\n" +
    "}";
string test3 = "{ int i; i = 2;}";

ILexerBehavior lexerBehavior = new LexerBehavior();
ILexer lexer = new Lexer(lexerBehavior);
NodeFactory nodeFactory = new NodeFactory(lexerBehavior);
Node node = nodeFactory.CreateNode();
TestParserBehavior parserBehavior = new(lexer,node);
TestParser parser = new (parserBehavior);
parser.Parse(test3.AsMemory());
parser.Move();
parser.Program();