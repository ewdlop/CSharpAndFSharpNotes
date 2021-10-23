namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record ElseStatement(IExpression Expression, IStatement Statement1, IStatement Statement2):Statement
    {
        public override void Generate(int b, int a)
        {
            int Label1 = NewLabel();
            int Label2 = NewLabel();
            Expression.Jumping(0, Label2);
            EmitLabel(Label1);
            Statement1.Generate(Label1, a);
            Emit($"goto L{a}");
            EmitLabel(Label2);
            Statement2.Generate(Label2, a);
        }
    }
}
