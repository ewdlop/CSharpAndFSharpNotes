using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokenizer;
using System;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers;

public interface ILexer
{
    IReadOnlyTokenizer ReadOnlyLexerCharacterReader { get; }
    void Lex(ReadOnlyMemory<char> characters);
    Tokens.Token? Scan();
    int LexLine { get; }
}
