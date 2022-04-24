using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behaviors;
using System;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers;

public interface ILexer
{
    IReadOnlyLexerBehavior ReadOnlyLexerCharacterReader { get; }
    void Lex(ReadOnlyMemory<char> characters);
    Tokens.Token Scan();
    int LexLine { get; }
}
