using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokenizer;

public class Tokenizer : ITokenizer
{
    private Dictionary<string, WordToken> WordTokens { get; init; } = new();
    public ReadOnlyMemory<char> _readingCharactrs;
    public ReadOnlyMemory<char>? ReadingCharacters => _readingCharactrs;
    public int Line { get; private set; }
    public char? Peek { get; private set; } = IReadOnlyTokenizer.EmptySpace;
    public IReadOnlyDictionary<string, WordToken> ReadOnlyWordTokens => WordTokens;
    public void Lex(ReadOnlyMemory<char> characters) => _readingCharactrs = characters;
    public virtual void ResetPeek() => Peek = IReadOnlyTokenizer.EmptySpace;
    public int index = -1;
    public virtual (bool Read, char? Peek) TryReadChar()
    {
        if (++index < _readingCharactrs.Length)
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
    public virtual bool IsPeekEmptySpace() => IsPeek(IReadOnlyTokenizer.EmptySpace);
    public virtual void Reserve(WordToken wordToken) => WordTokens.Add(wordToken.Lexeme, wordToken);
    public virtual Token? Scan()
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

        if (Peek is null) return null;

        (bool found, Token? token) = TryScanOperationToken(Peek.Value);
        if (found) return token;

        
        (bool numberTokenFound, Token? numberToken) = TryScanNumberToken(Peek.Value);
        if (numberTokenFound) return numberToken;

        (bool wordTokenFound, Token? wordToken) = TryScanWordToken(Peek.Value);
        if (wordTokenFound) return wordToken;

        Token undefineToken = new(Peek.Value);
        ResetPeek();
        return undefineToken;
    }

    private (bool found, Token? token) TryScanOperationToken(char peek) => peek switch
    {
        '&' => TryPeekAhead('&') ? (true, WordToken.AND) : (true, new Token('&')),
        '|' => TryPeekAhead('|') ? (true, WordToken.OR) : (true, new Token('|')),
        '=' => TryPeekAhead('=') ? (true, WordToken.EQUAL) : (true, new Token('=')),
        '!' => TryPeekAhead('=') ? (true, WordToken.NOT_EQUAL) : (true, new Token('!')),
        '<' => TryPeekAhead('=') ? (true, WordToken.LESS_OR_EQUAL) : (true, WordToken.LESS_THAN),
        '>' => TryPeekAhead('=') ? (true, WordToken.GREATER_OR_EQUAL) : (true, WordToken.GREATER_THAN),
        _ => (false, null)
    };

    private (bool found, Token? token) TryScanNumberToken(char peek)
    {
        if (char.IsDigit(peek))
        {
            int number = 0;
            do
            {
                number = 10 * number + Convert.ToInt32(Peek.ToString(), 10);
                (_, Peek) = TryReadChar();
            } while (Peek is not null && char.IsDigit(Peek.Value));
            if (!IsPeek('.'))
            {
                return (true, new NumberToken(number));
            }
            float decimalNumber = number;
            float tenExponent = 10;
            for ((_, Peek) = TryReadChar(); Peek is not null && char.IsDigit(Peek.Value); (_, Peek) = TryReadChar())
            {
                decimalNumber += Convert.ToInt32(Peek.ToString(), 10) / tenExponent;
                tenExponent *= 10;
            }
            return (true, new RealNumberToken(decimalNumber));
        }
        return (false, null);
    }

    private (bool found, Token? token) TryScanWordToken(char peek)
    {
        if (char.IsLetter(peek))
        {
            StringBuilder stringBuilder = new();
            do
            {
                stringBuilder.Append(Peek);
                (_, Peek) = TryReadChar();
            } while (Peek is not null && char.IsLetterOrDigit(Peek.Value));
            string word = stringBuilder.ToString();
            if (WordTokens.TryGetValue(word, out WordToken? wordToken))
            {
                return (true, wordToken);
            }
            else
            {
                wordToken = new WordToken(word, TokenTag.ID);
                WordTokens.TryAdd(word, wordToken);
                return (true, wordToken);
            }
        }
        return (false, null);
    }
}
