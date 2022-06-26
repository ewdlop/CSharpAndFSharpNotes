using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokenizer;

public interface ITokenizer : IReadOnlyTokenizer
{
    ReadOnlyMemory<char>? ReadingCharacters { get; }
    void ResetPeek();
    (bool Read, char? Peek) TryReadChar();
    bool TryPeekAhead(char c);
    void Reserve(WordToken wordToken);
    void Lex(ReadOnlyMemory<char> charactrs);
    Token? Scan();
}
