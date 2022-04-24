using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behaviors;

public interface ILexerBehavior : IReadOnlyLexerBehavior
{
    ReadOnlyMemory<char>? ReadingCharacters { get; }
    void ResetPeek();
    (bool Read, char? Peek) TryReadChar();
    bool TryPeekAhead(char c);
    void Reserve(WordToken wordToken);
    void Lex(ReadOnlyMemory<char> charactrs);
    Token Scan();
}
