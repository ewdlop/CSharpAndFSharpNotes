using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface IReadonlyExpression
    {
        TypeToken TypeToken { get; init; }
    }
}
