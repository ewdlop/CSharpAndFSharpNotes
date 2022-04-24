using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers;
using Environment = CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Environment;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behaviors;

public class TestParserBehavior : ParserBehavior<Environment, Statement, Expression>
{
    public TestParserBehavior(ILexer lexer, Node emitterNode) : base(lexer, emitterNode)
    {
    }
}