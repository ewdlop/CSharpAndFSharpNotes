namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record SequenceStatement(IStatement Statement1, IStatement Statement2): Statement
    {
        public override void Generate(int b, int a)
        {
            if(Statement1 == NullStatement)
            {
                Statement2.Generate(b, a);
            }
            else if( Statement2 == NullStatement)
            {
                Statement1.Generate(b, a);
            }
            else
            {
                int label = NewLabel();
                Statement1.Generate(b, a);
                EmitLabel(label);
                Statement2.Generate(b, a);
            }
        }
    }
}
