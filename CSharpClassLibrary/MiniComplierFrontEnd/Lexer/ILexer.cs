namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer
{
    public interface ILexer
    {
        IReadOnlyLexerCharacterReader ReadOnlyLexerCharacterReader { get; }
        Tokens.Token Scan();
    }
}
