using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers
{
    public interface IParser<T1, T2, T3>
        where T1 : IEnvironment, new()
        where T2 : IStatement, new()
        where T3 : IExpression, new()
    {
        void Move();
        void Match(int tag);
    }
}
