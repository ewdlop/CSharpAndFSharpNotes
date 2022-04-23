using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behavior;

public interface ILexerBehavior : IReadOnlyLexerBehavior
{
    void ResetPeek();
    void ReadChar();
    bool ReadChar(char c);
    void Reserve(WordToken wordToken);
    Token Scan();
}
