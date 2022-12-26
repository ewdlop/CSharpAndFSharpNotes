namespace Parsers.A;

public class Tokenizer
{
    public static Stack<Token> Tokenize(string expression)
    {
        // Initialize the list of tokens
        Stack<Token> tokens = new Stack<Token>();

        // Iterate over the characters in the expression
        for (int i = 0; i < expression.Length; i++)
        {
            // Get the current character
            char c = expression[i];

            // If the character is a digit, add it to the current number token
            if (char.IsDigit(c))
            {
                if (tokens.TryPeek(out Token? token) && token is not null && token.Type == TokenType.Number)
                {
                    if(tokens.TryPop(out var _))
                    {
                        Token newToken = token with
                        {
                            Value = $"{token.Value}{c}"
                        };
                        tokens.Push(newToken);
                    }
                }
                else
                {
                    token = new Token(TokenType.Number, c.ToString());
                    tokens.Push(token);
                }
            }
            // If the character is a space, skip it
            else if (char.IsWhiteSpace(c))
            {
                continue;
            }
            // Otherwise, add a new symbol token for the character
            else
            {
                tokens.Push(new Token(TokenType.Symbol, c.ToString()));
            }
        }

        // Return the list of tokens
        return tokens;
    }
}