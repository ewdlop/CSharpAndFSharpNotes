namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

public interface INode : IReadOnlyNode
{
    void Error(string error);
    int NewLabel();
}
