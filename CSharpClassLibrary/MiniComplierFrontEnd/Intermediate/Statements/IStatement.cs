using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public interface IStatement : IReadOnlyStatement
{
    void Generate(int begin, int after);
    void Init(IExpression expression, IStatement statement);
    ILabelEmitter Emitter { get; }
}
