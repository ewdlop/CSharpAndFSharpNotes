using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behavior
{
    public interface ILexerBehavior : IReadOnlyLexerBehavior
    {
        public const char EmptySpace = ' ';
        void ResetPeek();
        void ReadChar();
        bool ReadChar(char c);
        void Reserve(WordToken wordToken);
        bool IsPeek(char c);
        bool IsPeekEmptySpace();
        Token Scan();
    }
}
