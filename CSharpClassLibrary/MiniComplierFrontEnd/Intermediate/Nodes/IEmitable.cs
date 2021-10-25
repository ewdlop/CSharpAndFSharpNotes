namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes
{
    public interface IEmitable
    {
        void EmitLabel(int i);
        void Emit(string s);
    }
}
