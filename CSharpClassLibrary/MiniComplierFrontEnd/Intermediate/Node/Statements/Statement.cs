namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record Statement: Node, IStatement
    {
        public static readonly IStatement NullStatement = new Statement();
        public static readonly IStatement EnclosingStatement = NullStatement;
        public int After { get; protected set; }
        public virtual void Generate(int b, int a) { }
    }
}
