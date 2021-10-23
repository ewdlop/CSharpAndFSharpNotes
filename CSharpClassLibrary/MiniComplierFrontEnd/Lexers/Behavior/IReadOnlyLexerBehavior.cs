using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behavior
{
    public interface IReadOnlyLexerBehavior
    {
        IReadOnlyDictionary<string, WordToken> ReadOnlyWordTokens { get; }
        int Line { get; }
        char Peek { get; }
    }
}
