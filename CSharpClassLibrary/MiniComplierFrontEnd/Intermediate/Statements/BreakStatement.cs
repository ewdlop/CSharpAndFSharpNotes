using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements
{
    public class BreakStatement:Statement
    {
        public BreakStatement(Node node) : base(node) {}
        public IStatement Statement { get; init; } = EnclosingStatement;
        public override void Generate(int b, int a) => _node.Emit($"go to L{Statement.After}");
    }
}
