using Microsoft.Extensions.DependencyInjection;
using CSharpClassLibrary.MiniComplierFrontEnd.Parsers;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behavior;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behavior;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Service
{
    public static class MiniComplieerFrontEndService
    {
        public static IServiceCollection AddMiniComplieerFrontEndService(this IServiceCollection services
              /*, Action<X> configure*/)
        {
            //var options = new Options();
            //configure(options);

            return services
                .AddScoped<IParser<Environment, Statement, Expression>, Parser<Environment, Statement, Expression>>()
                .AddScoped<IParserBehavior<Environment, Statement, Expression>, ParserBehavior<Environment, Statement, Expression>>()
                .AddScoped<ILexer, Lexer>()
                .AddScoped<ILexerBehavior, LexerBehavior>()
                .AddScoped<INode, Node>()
                .AddScoped<IEnvironment, Environment>()
                .AddScoped<IStatement, Statement>()
                .AddScoped<IExpression, Expression>();
        }
    }
}
