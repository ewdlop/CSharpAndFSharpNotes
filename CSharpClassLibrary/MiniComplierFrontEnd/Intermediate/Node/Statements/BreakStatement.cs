namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record BreakStatement(IStatement Statement):Statement
    {
        public BreakStatement():this(EnclosingStatement){}
        public override void Generate(int b, int a) => Emit($"go to L{Statement.After}");
    }
}
