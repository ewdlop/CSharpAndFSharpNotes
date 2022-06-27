using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokenizer;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;
using System;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers;

public class Lexer : ILexer
{
    private readonly ITokenizer _tokenizer;
    public IReadOnlyTokenizer ReadOnlyLexerCharacterReader => _tokenizer;
    public Lexer(ITokenizer lexerBehavior)
    {
        _tokenizer = lexerBehavior;
        _tokenizer.Reserve(new("if", TokenTag.IF));
        _tokenizer.Reserve(new("else", TokenTag.ELSE));
        _tokenizer.Reserve(new("while", TokenTag.WHILE));
        _tokenizer.Reserve(new("do", TokenTag.DO));
        _tokenizer.Reserve(new("break", TokenTag.BREAK));
        _tokenizer.Reserve(new("null", TokenTag.NULL));
        _tokenizer.Reserve(WordToken.TRUE);
        _tokenizer.Reserve(WordToken.FALSE);
        _tokenizer.Reserve(TypeToken.INT);
        _tokenizer.Reserve(TypeToken.FLOAT);
        _tokenizer.Reserve(TypeToken.CHAR);
        _tokenizer.Reserve(TypeToken.BOOL);
    }
    public void Lex(ReadOnlyMemory<char> characters) => _tokenizer.Lex(characters);
    public Token? Scan() => _tokenizer.Scan();
    public int LexLine => _tokenizer.Line;
}
