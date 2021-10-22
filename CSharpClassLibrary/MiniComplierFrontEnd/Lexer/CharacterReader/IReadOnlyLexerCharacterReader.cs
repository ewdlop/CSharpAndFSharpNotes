using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer
{
    public interface IReadOnlyLexerCharacterReader
    {
        IReadOnlyDictionary<string, WordToken> ReadOnlyWordTokens { get; }
        int Line { get; }
        char Peek { get; }
    }
}
