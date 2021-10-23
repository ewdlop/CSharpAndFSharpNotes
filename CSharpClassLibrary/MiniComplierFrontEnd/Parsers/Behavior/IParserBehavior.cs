using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behavior
{
    public interface IParserBehavior<T1, T2,T3> : IReadOnlyParserBehavior<T1>
        where T1 : IEnvironment, new()
        where T2 : IStatement, new()
        where T3 : IExpression, new()
    {
        void Move();
        void Match(int tag);
        void Error(string s);
        void Program();
        void Declare();
        TypeToken Type();
        TypeToken Dims();
        T2 Block();
        T2 Statements();
        T2 Statement();
        T2 Assign();
        T3 Bool();
        T3 Join();
        T3 Equality();
        T3 Relation();
        T3 Expression();
        T3 Term();
        T3 Unary();
        T3 Factor();
        AccessingOperationExpression Offset(IdExpression id);
    }
}
