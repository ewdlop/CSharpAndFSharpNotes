namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface IEmitable
    {
        void EmitLabel(int i);
        void Emit(string s);
    }
}
