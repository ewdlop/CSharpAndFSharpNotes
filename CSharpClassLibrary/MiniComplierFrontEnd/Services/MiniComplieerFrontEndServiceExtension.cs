using Microsoft.Extensions.DependencyInjection;
using CSharpClassLibrary.MiniComplierFrontEnd.Parsers;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behaviors;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokenizer;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Services;

public static class MiniComplieerFrontEndServiceExtension
{
    public static IServiceCollection AddMiniComplieerFrontEnd(this IServiceCollection services
          /*, Action<X> configure*/)
    {
        //var options = new Options();
        //configure(options);

        return services
            .AddSingleton<NodeFactory>()
            .AddScoped<ITokenizer, Tokenizer>()
            .AddScoped<ILexer, Lexer>()
            .AddScoped<INode, Node>((serviceProvideer) => {
                return serviceProvideer.GetService<NodeFactory>()?.CreateNode();
            })
            .AddScoped<IParserBehavior<Environment, Statement, Expression>, ParserBehavior<Environment, Statement, Expression>>()
            .AddScoped<IParser<Environment, Statement, Expression>, Parser<Environment, Statement, Expression>>();
    }
}
