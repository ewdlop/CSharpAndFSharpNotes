using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements
{
    public interface IStatement : IReadOnlyStatement
    {
        void Generate(int b, int a);
        void Init(IExpression expression, IStatement statement);
    }
}
