using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behaviors;
using Environment = CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Environment;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers;

public class TestParser : Parser<Environment, Statement, Expression>
{
    public TestParser(IParserBehavior<Environment, Statement, Expression> parserBehavior) : base(parserBehavior)
    {
    }
}