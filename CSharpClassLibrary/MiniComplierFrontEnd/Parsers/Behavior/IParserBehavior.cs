using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behavior;

public interface IParserBehavior<T1, T2,T3> : IReadOnlyParserBehavior<T1>
    where T1 : IEnvironment
    where T2 : IStatement
    where T3 : IExpression
{
    void Move();
    void Match(int tag);
    void Error(string s);
    void Program();
    void Declare();
    TypeToken Type();
    TypeToken Dims(TypeToken typeToken);
    T2 Block();
    T2 Statements();
    T2 Statement();
    T2 Assign();
    T3 Boolean();
    T3 Join();
    T3 Equality();
    T3 Relation();
    T3 Expression();
    T3 Term();
    T3 Unary();
    T3 Factor();
    AccessingOperationExpression Offset(IdExpression id);
}
