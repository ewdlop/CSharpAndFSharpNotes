using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public interface IReadOnlyExpression
{
    TypeToken TypeToken { get; }
}
