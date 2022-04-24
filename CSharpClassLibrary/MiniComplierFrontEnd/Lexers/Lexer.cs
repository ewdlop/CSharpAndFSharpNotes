using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behaviors;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using System;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers;

public class Lexer : ILexer
{
    private readonly ILexerBehavior _lexerBehavior;
    public IReadOnlyLexerBehavior ReadOnlyLexerCharacterReader => _lexerBehavior;
    public Lexer(ILexerBehavior lexerBehavior)
    {
        _lexerBehavior = lexerBehavior;
        _lexerBehavior.Reserve(new("if", TokenTag.IF));
        _lexerBehavior.Reserve(new("else", TokenTag.ELSE));
        _lexerBehavior.Reserve(new("while", TokenTag.WHILE));
        _lexerBehavior.Reserve(new("do", TokenTag.DO));
        _lexerBehavior.Reserve(new("break", TokenTag.BREAK));
        _lexerBehavior.Reserve(new("null", TokenTag.NULL));
        _lexerBehavior.Reserve(WordToken.TRUE);
        _lexerBehavior.Reserve(WordToken.FALSE);
        _lexerBehavior.Reserve(TypeToken.INT);
        _lexerBehavior.Reserve(TypeToken.FLOAT);
        _lexerBehavior.Reserve(TypeToken.CHAR);
        _lexerBehavior.Reserve(TypeToken.BOOL);
    }
    public void Lex(ReadOnlyMemory<char> characters) => _lexerBehavior.Lex(characters);
    public Token Scan() => _lexerBehavior.Scan();
    public int LexLine => _lexerBehavior.Line;
}
