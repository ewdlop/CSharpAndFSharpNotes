using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public interface IReadOnlyExpression
{
    TypeToken TypeToken { get; }
}
