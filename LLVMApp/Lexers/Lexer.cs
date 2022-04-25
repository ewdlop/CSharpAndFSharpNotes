using System.Text;

namespace LLVMApp.Lexers;
public class Lexer : ILexer
{
    private const int EOF = -1;

    private readonly TextReader _reader;
    private readonly StringBuilder _identifierBuilder = new();

    private readonly StringBuilder _numberBuilder = new();

    private readonly Dictionary<char, int>? _binaryOperationPrecedence;
    public int? CurrentToken { get; private set; }

    private int _currentCharCode = ' ';

    public string? LastIdentifier { get; private set; }

    public double? LastNumberValue { get; private set; }

    public Lexer(TextReader reader, Dictionary<char, int>? binOpPrecedence)
    {
        _reader = reader;
        _binaryOperationPrecedence = binOpPrecedence;
    }

    public int? GetNextToken()
    {
        if(_reader is null) return null; 
        while (char.IsWhiteSpace((char)_currentCharCode))
        {
            _currentCharCode = _reader.Read(); ;
        }
        if (char.IsLetter((char)_currentCharCode)) // identifier: [a-zA-Z][a-zA-Z0-9]*
        {
            _identifierBuilder.Append((char)_currentCharCode);
            while (char.IsLetterOrDigit((char)(_currentCharCode = _reader.Read())))
            {
                _identifierBuilder.Append((char)_currentCharCode);
            }
            LastIdentifier = _identifierBuilder.ToString();
            _identifierBuilder.Clear();

            if (string.Equals(LastIdentifier, "def", StringComparison.Ordinal))
            {
                CurrentToken = (int)Token.DEF;
            }
            else if (string.Equals(LastIdentifier, "extern", StringComparison.Ordinal))
            {
                CurrentToken = (int)Token.EXTERN;
            }
            else if (string.Equals(LastIdentifier, "if", StringComparison.Ordinal))
            {
                CurrentToken = (int)Token.IF;
            }
            else if (string.Equals(LastIdentifier, "then", StringComparison.Ordinal))
            {
                CurrentToken = (int)Token.THEN;
            }
            else if (string.Equals(LastIdentifier, "else", StringComparison.Ordinal))
            {
                CurrentToken = (int)Token.ELSE;
            }
            else if (string.Equals(LastIdentifier, "for", StringComparison.Ordinal))
            {
                CurrentToken = (int)Token.FOR;
            }
            else if (string.Equals(LastIdentifier, "in", StringComparison.Ordinal))
            {
                CurrentToken = (int)Token.IN;
            }
            else
            {
                CurrentToken = (int)Token.IDENTIFIER;
            }
        }
        // Number: [0 - 9.]+
        if (char.IsDigit((char)_currentCharCode) || _currentCharCode == '.')
        {
            do
            {
                _numberBuilder.Append((char)_currentCharCode);
                _currentCharCode = _reader.Read();
            } while (char.IsDigit((char)_currentCharCode) || _currentCharCode == '.');

            LastNumberValue = double.Parse(_numberBuilder.ToString());
            _numberBuilder.Clear();
            CurrentToken = (int)Token.NUMBER;
            return CurrentToken;
        }
        if (_currentCharCode == '#')
        {
            // Comment until end of line.
            do
            {
                _currentCharCode = _reader.Read();
            } while (_currentCharCode != EOF && _currentCharCode != '\n' && _currentCharCode != '\r');

            if (_currentCharCode != EOF)
            {
                return GetNextToken();
            }
        }
        if (_currentCharCode == EOF)
        {
            CurrentToken = _currentCharCode;
            return (int)Token.EOF;
        }

        CurrentToken = _currentCharCode;
        _currentCharCode = _reader.Read();
        return _currentCharCode;
    }

    public int? GetTokenPrecedence() => CurrentToken is null
            ? null
            : _binaryOperationPrecedence?.TryGetValue((char)CurrentToken, out int tokenPrecedence)
                ?? false ? tokenPrecedence : null;
}
