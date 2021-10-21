using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer
{
    public class LexerCharacterReader : ILexerCharacterReader
    {
        private Dictionary<string, WordToken> WordTokens { get; init; } = new();
        public int Line { get; private set; }
        public char Peek { get; private set; } = ILexerCharacterReader.EmptySpace;
        public IReadOnlyDictionary<string, WordToken> ReadOnlyWordTokens => WordTokens;
        public virtual void ResetPeek() => Peek = ILexerCharacterReader.EmptySpace;
        public virtual void ReadChar() => Peek = Convert.ToChar(Console.Read());
        public virtual bool ReadChar(char c)
        {
            ReadChar();
            if (Peek.CompareTo(c) != 0)
            {
                return false;
            }
            ResetPeek();
            return true;
        }
        public virtual bool IsPeek(char c) => Peek.CompareTo(c) == 0;
        public virtual bool IsPeekEmptySpace() => IsPeek(ILexerCharacterReader.EmptySpace);
        public virtual void Reserve(WordToken wordToken) => WordTokens.Add(wordToken.Lexeme, wordToken);
        public virtual Token.Token Scan()
        {
            for (; ; ReadChar())
            {
                if (IsPeekEmptySpace() || IsPeek('\t'))
                {
                    //continue;
                }
                else if (IsPeek('\n'))
                {
                    Line++;
                }
                else
                {
                    break;
                }
            }
            switch (Peek)
            {
                case '&':
                    if (ReadChar('&'))
                    {
                        return WordToken.AND;
                    }
                    else
                    {
                        return new Token.Token('&');
                    }
                case '|':
                    if (ReadChar('|'))
                    {
                        return WordToken.OR;
                    }
                    else
                    {
                        return new Token.Token('|');
                    }
                case '=':
                    if (ReadChar('='))
                    {
                        return WordToken.EQUAL;
                    }
                    else
                    {
                        return new Token.Token('=');
                    }
                case '!':
                    if (ReadChar('='))
                    {
                        return WordToken.NOT_EQUAL;
                    }
                    else
                    {
                        return new Token.Token('!');
                    }
                case '<':
                    if (ReadChar('='))
                    {
                        return WordToken.LESS_OR_EQUAL;
                    }
                    else
                    {
                        return WordToken.LESS_THAN;
                    }
                case '>':
                    if (ReadChar('='))
                    {
                        return WordToken.GREATER_OR_EQUAL;
                    }
                    else
                    {
                        return WordToken.GREATER_THAN;
                    }
            }

            if (char.IsDigit(Peek))
            {
                int number = 0;
                do
                {
                    number = (10 * number) + Convert.ToInt32(Peek.ToString(), 10);
                    ReadChar();
                } while (char.IsDigit(Peek));
                if (!IsPeek('.'))
                {
                    return new NumberToken(number);
                }
                float decimalNumber = number;
                float tenExponent = 10;
                for (; ; )
                {
                    ReadChar();
                    if (!char.IsDigit(Peek))
                    {
                        break;
                    }
                    decimalNumber += Convert.ToInt32(Peek.ToString(), 10) / tenExponent;
                    tenExponent *= 10;
                }
            }

            if (char.IsLetter(Peek))
            {
                var stringBuilder = new StringBuilder();
                do
                {
                    stringBuilder.Append(Peek);
                    ReadChar();
                } while (char.IsLetterOrDigit(Peek));
                string word = stringBuilder.ToString();
                if (WordTokens.TryGetValue(word, out WordToken wordToken))
                {
                    return wordToken;
                }
                else
                {
                    wordToken = new WordToken(word, TokenTag.ID);
                    WordTokens.TryAdd(word, wordToken);
                    return wordToken;
                }
            }

            var token = new Token.Token(Peek);
            //var token = new Token.Token() { Tag = (this as ILexer).Peek - '0' };
            ResetPeek();
            return token;
        }
    }
}
