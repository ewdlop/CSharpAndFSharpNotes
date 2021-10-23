using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behavior;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers
{
    public interface ILexer
    {
        IReadOnlyLexerBehavior ReadOnlyLexerCharacterReader { get; }
        Tokens.Token Scan();
        int LexLine { get; }
    }
}
