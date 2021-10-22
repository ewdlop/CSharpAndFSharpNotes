using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer
{
    public class Lexer : ILexer
    {
        private readonly ILexerCharacterReader _lexerCharacterReader;
        public IReadOnlyLexerCharacterReader ReadOnlyLexerCharacterReader => _lexerCharacterReader;
        public Lexer()
        {
            _lexerCharacterReader = new LexerCharacterReader();
            _lexerCharacterReader.Reserve(new("if", TokenTag.IF));
            _lexerCharacterReader.Reserve(new("else", TokenTag.ELSE));
            _lexerCharacterReader.Reserve(new("while", TokenTag.WHILE));
            _lexerCharacterReader.Reserve(new("do", TokenTag.DO));
            _lexerCharacterReader.Reserve(new("break", TokenTag.BREAK));
            _lexerCharacterReader.Reserve(WordToken.TRUE);
            _lexerCharacterReader.Reserve(WordToken.FALSE);
            _lexerCharacterReader.Reserve(TypeToken.INT);
            _lexerCharacterReader.Reserve(TypeToken.FLOAT);
            _lexerCharacterReader.Reserve(TypeToken.CHAR);
            _lexerCharacterReader.Reserve(TypeToken.BOOL);
        }
        public Token Scan() => _lexerCharacterReader.Scan();
    }
}
