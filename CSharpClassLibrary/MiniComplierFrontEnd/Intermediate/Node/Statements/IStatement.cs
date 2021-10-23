namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public interface IStatement
    {
        int After { get;}
        void Generate(int b, int a);
    }
}
