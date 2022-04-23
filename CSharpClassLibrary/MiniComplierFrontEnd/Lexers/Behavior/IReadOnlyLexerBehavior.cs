using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behavior;

public interface IReadOnlyLexerBehavior
{
    public const char EmptySpace = ' ';
    IReadOnlyDictionary<string, WordToken> ReadOnlyWordTokens { get; }
    int Line { get; }
    char Peek { get; }
    bool IsPeek(char c);
    bool IsPeekEmptySpace();
}
