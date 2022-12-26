// See https://aka.ms/new-console-template for more information
using Parsers.A;
using System.Linq.Expressions;

Console.WriteLine("Hello, World!");

string expression = "2 * 3 + 4";

// Create a list of tokens from the expression
var tokens = Tokenizer.Tokenize(expression);

// Parse the list of tokens into an abstract syntax tree (AST)
var ast = Parser.Parse<int>(tokens);

if (ast is null) return;

// Create a visitor for evaluating the AST
var visitor = new Evaluator<int>();

// Evaluate the AST and print the result
Console.WriteLine(ast.Accept(visitor));

expression = "2 * 3 + 4";
//Console.WriteLine(Parsers.B.Parser.Parse<int>(expression).Compile().DynamicInvoke());

var expr = TestMethod(i => i + 1);

Console.WriteLine(Expression.Lambda<Func<int, int>>(expr.Body, expr.Parameters).Compile().Invoke(1));
static LambdaExpression TestMethod(Expression<Func<int, int>> expression)
{
    return expression;
}