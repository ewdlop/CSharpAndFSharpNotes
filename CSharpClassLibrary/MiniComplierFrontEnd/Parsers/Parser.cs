using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behavior;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers
{
    public class Parser<T1,T2,T3> : IParser<T1, T2, T3>
        where T1 : IEnvironment
        where T2 : IStatement
        where T3 : IExpression
    {
        private readonly IParserBehavior<T1, T2, T3> _parserBehavior;
        public Parser(ParserBehavior<T1, T2, T3> parserBehavior)
        {
            _parserBehavior = parserBehavior;
        }
        public void Move() => _parserBehavior.Move();
        public void Match(int tag) => _parserBehavior.Match(tag);
    }
}
