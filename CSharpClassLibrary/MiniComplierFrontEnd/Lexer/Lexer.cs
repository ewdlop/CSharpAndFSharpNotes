using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer
{
    public interface IScan
    {
        public Token.Token Scan();
    }
    public interface ILexer : IScan
    {
        private const char EmptySpace = ' ';
        char Peek { get; set; }
        Dictionary<string, WordToken> WordTokens { get; init; }
        void Reserve(WordToken wordToken) => WordTokens.Add(wordToken.Lexeme, wordToken);
        void ResetPeek() => Peek = EmptySpace;
        void ReadChar() => Convert.ToChar(Console.Read());
        bool ReadChar(char c)
        {
            ReadChar();
            if (Peek.CompareTo(c) != 0)
            {
                return false;
            }
            ResetPeek();
            return true;
        }
    }
    public class Lexer : ILexer
    {
        private const char EmptySpace = ' ';
        public static int LINE { get; set; } = 1;
        char ILexer.Peek { get; set; } = EmptySpace;
        Dictionary<string, WordToken> ILexer.WordTokens { get; init; } = new();
        public Lexer()
        {
            (this as ILexer).Reserve(new("if", TokenTag.IF));
            (this as ILexer).Reserve(new("else", TokenTag.ELSE));
            (this as ILexer).Reserve(new("while", TokenTag.WHILE));
            (this as ILexer).Reserve(new("do", TokenTag.DO));
            (this as ILexer).Reserve(new("break", TokenTag.BREAK));
            (this as ILexer).Reserve(WordToken.TRUE);
            (this as ILexer).Reserve(WordToken.FALSE);
            (this as ILexer).Reserve(TypeToken.INT);
            (this as ILexer).Reserve(TypeToken.FLOAT);
            (this as ILexer).Reserve(TypeToken.CHAR);
            (this as ILexer).Reserve(TypeToken.BOOL);
        }

        public Token.Token Scan()
        {
            for (; ; (this as ILexer).ReadChar())
            {
                if ((this as ILexer).Peek.CompareTo(EmptySpace) == 0 || (this as ILexer).Peek.CompareTo('\t') == 0)
                {
                    //continue;
                }
                else if ((this as ILexer).Peek.CompareTo(EmptySpace) == '\n')
                {
                    LINE++;
                }
                else
                {
                    break;
                }
            }
            switch ((this as ILexer).Peek)
            {
                case '&':
                    if ((this as ILexer).ReadChar('&'))
                    {
                        return WordToken.AND;
                    }
                    else
                    {
                        return new Token.Token('&');
                    }
                case '|':
                    if ((this as ILexer).ReadChar('|'))
                    {
                        return WordToken.OR;
                    }
                    else
                    {
                        return new Token.Token('|');
                    }
                case '=':
                    if ((this as ILexer).ReadChar('='))
                    {
                        return WordToken.EQUAL;
                    }
                    else
                    {
                        return new Token.Token('=');
                    }
                case '!':
                    if ((this as ILexer).ReadChar('='))
                    {
                        return WordToken.NOT_EQUAL;
                    }
                    else
                    {
                        return new Token.Token('!');
                    }
                case '<':
                    if ((this as ILexer).ReadChar('='))
                    {
                        return WordToken.LESS_OR_EQUAL;
                    }
                    else
                    {
                        return WordToken.LESS_THAN;
                    }
                case '>':
                    if ((this as ILexer).ReadChar('='))
                    {
                        return WordToken.GREATER_OR_EQUAL;
                    }
                    else
                    {
                        return WordToken.GREATER_THAN;
                    }
            }
            if (char.IsDigit((this as ILexer).Peek))
            {
                int number = 0;
                do
                {
                    number = (10 * number) + Convert.ToInt32((this as ILexer).Peek.ToString(), 10);
                    (this as ILexer).ReadChar();
                } while (char.IsDigit((this as ILexer).Peek));
                if ((this as ILexer).Peek.CompareTo('.') != 0)
                {
                    return new NumberToken(number);
                }
                float decimalNumber = number;
                float tenExponent = 10;
                for (; ; )
                {
                    (this as ILexer).ReadChar();
                    if (!char.IsDigit((this as ILexer).Peek))
                    {
                        break;
                    }
                    decimalNumber += Convert.ToInt32((this as ILexer).Peek.ToString(), 10) / tenExponent;
                    tenExponent *= 10;
                }
            }
            if (char.IsLetter((this as ILexer).Peek))
            {
                var stringBuilder = new StringBuilder();
                do
                {
                    stringBuilder.Append((this as ILexer).Peek);
                    (this as ILexer).ReadChar();
                } while (char.IsLetterOrDigit((this as ILexer).Peek));
                string word = stringBuilder.ToString();
                if ((this as ILexer).WordTokens.TryGetValue(word, out WordToken wordToken))
                {
                    return wordToken;
                }
                else
                {
                    wordToken = new WordToken(word, TokenTag.ID);
                    (this as ILexer).WordTokens.TryAdd(word, wordToken);
                    return wordToken;
                }
            }
            var token = new Token.Token((this as ILexer).Peek);
            //var token = new Token.Token() { Tag = (this as ILexer).Peek - '0' };
            (this as ILexer).ResetPeek();
            return token;
        }
    }
}
