using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer
{
    public interface ILexerCharacterReader
    {
        public const char EmptySpace = ' ';
        IReadOnlyDictionary<string, WordToken> ReadOnlyWordTokens { get; }
        int Line { get; }
        char Peek { get; }
        void ResetPeek();
        void ReadChar();
        bool ReadChar(char c);
        void Reserve(WordToken wordToken);
        bool IsPeek(char c);
        bool IsPeekEmptySpace();
        Token.Token Scan();
    }
}
