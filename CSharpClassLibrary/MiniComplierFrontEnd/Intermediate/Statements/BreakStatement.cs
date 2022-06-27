using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public class BreakStatement:Statement
{
    public BreakStatement(Emitter node) : base(node) {}
    public IStatement Statement { get; init; } = EnclosingStatement;
    public override void Generate(int begin, int after) => _emitter.Emit($"go to L{Statement.After}");
}
