namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

public interface IReadOnlyNode : IEmitable
{
    int Lexline { get; }
}
