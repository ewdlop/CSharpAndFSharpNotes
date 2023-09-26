namespace ConsoleApp8;

public class Lexer(string input)
{
    private const char ZERO = '\0';
    private int position = 0;
    private char currentChar = input[0];
    public string Input => input;
    public char CurrentChar => currentChar;

    public Token GetNextToken()
    {
        while (char.IsWhiteSpace(CurrentChar))
        {
            Advance();
        }

        if (char.IsDigit(CurrentChar))
        {
            string value = string.Empty;
            while (char.IsDigit(CurrentChar))
            {
                value += CurrentChar;
                Advance();
            }
            return new Token(TokenType.Number, value);
        }

        if (CurrentChar == '+') { Advance(); return new Token(TokenType.Plus, "+"); }
        if (CurrentChar == '*') { Advance(); return new Token(TokenType.Times, "*"); }
        if (CurrentChar == '-') { Advance(); return new Token(TokenType.Minus, "-"); }
        if (CurrentChar == '/') { Advance(); return new Token(TokenType.Div, "/"); }
        if (CurrentChar == '(') { Advance(); return new Token(TokenType.LeftParenthesis, "("); }
        if (CurrentChar == ')') { Advance(); return new Token(TokenType.RightParenthesis, ")"); }
        // Add more token types as needed.

        if (position >= Input.Length) return new Token(TokenType.EOF, null);

        throw new Exception($"Unexpected character: {CurrentChar}");
    }

    private void Advance()
    {
        position++;
        currentChar = position >= Input.Length ? ZERO : Input[position];
    }
}