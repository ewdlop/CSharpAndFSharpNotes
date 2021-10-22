using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer
{
    public interface ILexerCharacterReader : IReadOnlyLexerCharacterReader
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
