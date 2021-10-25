using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements
{
    public interface IStatement : IReadOnlyStatement
    {
        void Generate(int begin, int after);
        void Init(IExpression expression, IStatement statement);
        INode Node { get; }
    }
}
