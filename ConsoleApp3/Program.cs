using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokenizer;
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
//string test2 = "{ float[100] a;\n" +
//    "while(true){\n" +
//    "a[50] = 5;\n" +
//    "}\n" +
//    "}";
//string test3 = "{ int i; i = 2;}";

ITokenizer tokenizer = new Tokenizer();
ILexer lexer = new Lexer(tokenizer);
NodeFactory nodeFactory = new NodeFactory(tokenizer);
Node node = nodeFactory.CreateNode();
TestParserBehavior parserBehavior = new(lexer, node);
TestParser parser = new(parserBehavior);
parser.Parse(test.AsMemory());
parser.Move();
parser.Program();


//await Parallel.ForEachAsync(Values(), async (x, y) => 
//{
//    Console.WriteLine($"{x}");
//    await Task.Delay(5000);
//    Console.WriteLine($"{x}+5000");
//});

//Console.WriteLine("Hello World!");

//async IAsyncEnumerable<int> Values()
//{
//    while(true)
//    {
//        await Task.Delay(1000);
//        yield return new System.Random().Next(0, 5);
//    }
//}


//int x = ParallelEnumerable.Range(0, 100000).OrderBy(x=>x).Where(p => { Console.WriteLine($"check:{p}"); return p == 3000; }).FirstOrDefault();
//Console.WriteLine(x);