namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

public interface ILabelEmitter : IReadonlyEmitter
{
    void Error(string error);
    int NewLabel();
}
