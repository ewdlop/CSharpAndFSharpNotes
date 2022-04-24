namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

public interface IReadOnlyNode : IEmit
{
    int Lexline { get; }
}
