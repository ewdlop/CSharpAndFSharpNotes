namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record IfStatement(IExpression Expression, IStatement Statement):Statement
    {
        public override void Generate(int b, int a)
        {
            int Label = NewLabel();
            Expression.Jumping(0, a);
            EmitLabel(Label);
            Statement.Generate(Label, a);
        }
    }
}
