namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record WhileStatement(IExpression Expression, IStatement Statement):Statement
    {
        public WhileStatement() : this(null, null){}
        public override void Generate(int b, int a)
        {
            After = a;
            Expression.Jumping(0, a);
            int label = NewLabel();
            EmitLabel(label);
            Statement.Generate(label, b);
            Emit($"goto L{b}");
        }
    }
}
