using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

public interface IReadOnlyEnvironment
{
    IReadOnlyDictionary<Token, IdExpression> TokenIdExpression { get; }
    IReadOnlyEnvironment PreviousEnvironment { get; }
    public IdExpression? Get(Token token);
}
