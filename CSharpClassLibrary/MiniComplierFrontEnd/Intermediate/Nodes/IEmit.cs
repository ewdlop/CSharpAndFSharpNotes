namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

public interface IEmit
{
    void EmitLabel(int i);
    void Emit(string s);
}
