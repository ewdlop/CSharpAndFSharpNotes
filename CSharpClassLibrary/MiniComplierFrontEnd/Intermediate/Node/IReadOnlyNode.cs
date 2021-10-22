namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface IReadOnlyNode : IEmitable
    {
        int Lexline { get; }
    }
}
