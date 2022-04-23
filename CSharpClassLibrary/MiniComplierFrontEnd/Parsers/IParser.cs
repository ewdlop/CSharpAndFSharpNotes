using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers;

public interface IParser<T1, T2, T3>
    where T1 : IEnvironment
    where T2 : IStatement
    where T3 : IExpression
{
    void Move();
    void Match(int tag);
}
