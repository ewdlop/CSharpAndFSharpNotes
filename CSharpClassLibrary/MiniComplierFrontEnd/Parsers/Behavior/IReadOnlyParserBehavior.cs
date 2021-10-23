using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behavior
{
    public interface IReadOnlyParserBehavior<T>
        where T : IEnvironment, new()
    {
        Token LookAheadToken { get; }
        T TopSymbol { get; }
        int Used { get; }
    }
}
