namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

public interface IEmitter
{
    void EmitLabel(int i);
    void Emit(string s);
}
