namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record DoStatement(IExpression Expression, IStatement Statement):Statement
    {
        public DoStatement() : this(null, null) { }
        public override void Generate(int b, int a)
        {
            After = a;
            int label = NewLabel();
            Statement.Generate(b, label);
            EmitLabel(label);
            Expression.Jumping(b, 0);
        }
    }
}
