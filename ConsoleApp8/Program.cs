using ConsoleApp8;

string input = "(3+5)*2";
Lexer lexer = new Lexer(input);
Parser<double> parser = new Parser<double>(lexer);
AST<double> tree = parser.Parse();

Evaluator<double> evaluator = new Evaluator<double>();
double result = tree.Accept(evaluator);
Console.WriteLine(result);  // Outputs: 16