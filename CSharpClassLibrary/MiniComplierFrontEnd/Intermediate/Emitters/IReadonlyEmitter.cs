namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

public interface IReadonlyEmitter : IEmitter
{
    int Lexline { get; }
}
