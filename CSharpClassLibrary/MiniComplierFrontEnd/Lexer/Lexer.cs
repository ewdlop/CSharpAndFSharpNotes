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
        public const char EmptySpace = ' ';
        char Peek { get; }
        Dictionary<string, WordToken> WordTokens { get;}
        void Reserve(WordToken wordToken);
        void ResetPeek();
        void ReadChar();
        bool ReadChar(char c);
    }
    public class Lexer : ILexer
    {
        private readonly Dictionary<string, WordToken> _wordTokens;
        private char _peek { get; set; }
        public static int LINE { get; set; } = 1;
        Dictionary<string, WordToken> ILexer.WordTokens => _wordTokens;
        public Lexer()
        {
            _wordTokens = new();
            _peek = ILexer.EmptySpace;
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
        char ILexer.Peek => _peek;
        void ILexer.Reserve(WordToken wordToken) => _wordTokens.Add(wordToken.Lexeme, wordToken);
        void ILexer.ResetPeek() => _peek = ILexer.EmptySpace;
        void ILexer.ReadChar() => Convert.ToChar(Console.Read());
        bool ILexer.ReadChar(char c)
        {
            (this as ILexer).ReadChar();
            if (_peek.CompareTo(c) != 0)
            {
                return false;
            }
            (this as ILexer).ResetPeek();
            return true;
        }
        public Token.Token Scan()
        {
            for (; ; (this as ILexer).ReadChar())
            {
                if (_peek.CompareTo(ILexer.EmptySpace) == 0 || _peek.CompareTo('\t') == 0)
                {
                    //continue;
                }
                else if (_peek.CompareTo(ILexer.EmptySpace) == '\n')
                {
                    LINE++;
                }
                else
                {
                    break;
                }
            }
            switch (_peek)
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
            if (char.IsDigit(_peek))
            {
                int number = 0;
                do
                {
                    number = (10 * number) + Convert.ToInt32(_peek.ToString(), 10);
                    (this as ILexer).ReadChar();
                } while (char.IsDigit(_peek));
                if (_peek.CompareTo('.') != 0)
                {
                    return new NumberToken(number);
                }
                float decimalNumber = number;
                float tenExponent = 10;
                for (; ; )
                {
                    (this as ILexer).ReadChar();
                    if (!char.IsDigit(_peek))
                    {
                        break;
                    }
                    decimalNumber += Convert.ToInt32(_peek.ToString(), 10) / tenExponent;
                    tenExponent *= 10;
                }
            }
            if (char.IsLetter(_peek))
            {
                var stringBuilder = new StringBuilder();
                do
                {
                    stringBuilder.Append(_peek);
                    (this as ILexer).ReadChar();
                } while (char.IsLetterOrDigit(_peek));
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
            var token = new Token.Token(_peek);
            //var token = new Token.Token() { Tag = (this as ILexer).Peek - '0' };
            (this as ILexer).ResetPeek();
            return token;
        }
    }
}
