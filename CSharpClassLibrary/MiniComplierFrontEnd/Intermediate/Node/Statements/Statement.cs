namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record Statement:Node
    {
        public static readonly Statement NullStatement = new();
        public static readonly Statement EnclosingStatement = NullStatement;
        public int After { get;}
        public virtual void Generate(int b, int a) { }
    }
}
