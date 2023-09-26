namespace ConsoleApp8;

public class Parser(Lexer lexer)
{
    private Token currentToken = lexer.GetNextToken();
    public Token CurrentToken => currentToken;
    // Corresponds to EXPRESSION -> TERM '+' EXPRESSION | TERM
    public AST Expression()
    {
        var node = Term();

        while (CurrentToken.Type == TokenType.Plus || CurrentToken.Type == TokenType.Minus)
        {
            var token = CurrentToken;
            if (token.Type == TokenType.Plus)
            {
                Eat(TokenType.Plus);
            }
            else if (token.Type == TokenType.Minus)
            {
                Eat(TokenType.Minus);
            }

            node = new BinOp(node, token, Term());
        }

        return node;
    }

    public AST Term()
    {
        var node = Factor();

        while (CurrentToken.Type == TokenType.Times || CurrentToken.Type == TokenType.Div)
        {
            Token token = CurrentToken;
            if (token.Type == TokenType.Times)
            {
                Eat(TokenType.Times);
            }
            else if (token.Type == TokenType.Div)
            {
                Eat(TokenType.Div);
            }

            node = new BinOp(node, token, Factor());
        }

        return node;
    }

    public AST Factor()
    {
        var token = CurrentToken;

        if (token.Type == TokenType.Number)
        {
            Eat(TokenType.Number);
            return new Num(token);
        }
        else if (token.Type == TokenType.LeftParenthesis)
        {
            Eat(TokenType.LeftParenthesis);
            var node = Expression();
            Eat(TokenType.RightParenthesis);
            return node;
        }

        throw new Exception("Syntax Error");
    }

    public AST Parse()
    {
        return Expression();
    }

    private void Eat(TokenType type)
    {
        if (currentToken.Type == type)
            currentToken = lexer.GetNextToken();
        else
            throw new Exception($"Token mismatch: expected {type}, got {currentToken.Type}");
    }
}
