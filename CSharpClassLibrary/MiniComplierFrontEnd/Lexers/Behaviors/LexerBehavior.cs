using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behaviors;

public class LexerBehavior : ILexerBehavior
{
    private Dictionary<string, WordToken> WordTokens { get; init; } = new();
    public ReadOnlyMemory<char> _readingCharactrs;
    public ReadOnlyMemory<char>? ReadingCharacters => _readingCharactrs;
    public int Line { get; private set; }
    public char? Peek { get; private set; } = IReadOnlyLexerBehavior.EmptySpace;
    public IReadOnlyDictionary<string, WordToken> ReadOnlyWordTokens => WordTokens;
    public void Lex(ReadOnlyMemory<char> characters) => _readingCharactrs = characters;
    public virtual void ResetPeek() => Peek = IReadOnlyLexerBehavior.EmptySpace;
    public int index = -1;
    public virtual (bool Read, char? Peek) TryReadChar()
    {
        if(++index < _readingCharactrs.Length)
        {
            return (true, _readingCharactrs.Span[index]);
        }
        return (false, null);
    }
    public virtual bool TryPeekAhead(char c)
    {
        (bool read, Peek) = TryReadChar();
        if (!read || !Peek.Equals(c)) return false;
        ResetPeek();
        return true;
    }
    public virtual bool IsPeek(char c) => Peek.Equals(c);
    public virtual bool IsPeekEmptySpace() => IsPeek(IReadOnlyLexerBehavior.EmptySpace);
    public virtual void Reserve(WordToken wordToken) => WordTokens.Add(wordToken.Lexeme, wordToken);
    public virtual Token Scan()
    {
        bool read = true;
        for (; read; (read, Peek) = TryReadChar())
        { 
            if (IsPeekEmptySpace() || IsPeek('\t'))
            {
                continue;
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
                if (TryPeekAhead('&'))
                {
                    return WordToken.AND;
                }
                else
                {
                    return new Token('&');
                }
            case '|':
                if (TryPeekAhead('|'))
                {
                    return WordToken.OR;
                }
                else
                {
                    return new Token('|');
                }
            case '=':
                if (TryPeekAhead('='))
                {
                    return WordToken.EQUAL;
                }
                else
                {
                    return new Token('=');
                }
            case '!':
                if (TryPeekAhead('='))
                {
                    return WordToken.NOT_EQUAL;
                }
                else
                {
                    return new Token('!');
                }
            case '<':
                if (TryPeekAhead('='))
                {
                    return WordToken.LESS_OR_EQUAL;
                }
                else
                {
                    return WordToken.LESS_THAN;
                }
            case '>':
                if (TryPeekAhead('='))
                {
                    return WordToken.GREATER_OR_EQUAL;
                }
                else
                {
                    return WordToken.GREATER_THAN;
                }
            default:
                break;
        }
        if (Peek is null) return null;
        if (char.IsDigit(Peek.Value))
        {
            int number = 0;
            do
            {
                number = 10 * number + Convert.ToInt32(Peek.ToString(), 10);
                (_, Peek) = TryReadChar();
            } while (char.IsDigit(Peek.Value));
            if (!IsPeek('.'))
            {
                return new NumberToken(number);
            }
            float decimalNumber = number;
            float tenExponent = 10;
            for ((_, Peek) = TryReadChar(); char.IsDigit(Peek.Value); (_, Peek) = TryReadChar())
            {
                decimalNumber += Convert.ToInt32(Peek.ToString(), 10) / tenExponent;
                tenExponent *= 10;
            }
            return new RealNumberToken(decimalNumber);
        }
        if (char.IsLetter(Peek.Value))
        {
            var stringBuilder = new StringBuilder();
            do
            {
                stringBuilder.Append(Peek);
                (_, Peek) = TryReadChar();
            } while (char.IsLetterOrDigit(Peek.Value));
            string word = stringBuilder.ToString();
            if (WordTokens.TryGetValue(word, out WordToken? wordToken))
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
        var token = new Token(Peek.Value);
        ResetPeek();
        return token;
    }
}
