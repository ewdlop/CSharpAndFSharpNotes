using ConsoleApp8;

string input = "(3+5)*2";
Lexer lexer = new Lexer(input);
Parser parser = new Parser(lexer);
AST tree = parser.Parse();

Evaluator evaluator = new Evaluator();
double result = tree.Accept(evaluator);
Console.WriteLine(result);  // Outputs: 16