using System.Numerics;

namespace ConsoleApp8;

public class Parser<T>(Lexer lexer) where T : INumber<T>
{
    private Token currentToken = lexer.GetNextToken();
    public Token CurrentToken => currentToken;
    public AST<T> Expression()
    {
        AST<T> node = Term();

        while (CurrentToken.Type == TokenType.Plus || CurrentToken.Type == TokenType.Minus)
        {
            Token token = CurrentToken;
            if (token.Type == TokenType.Plus)
            {
                Eat(TokenType.Plus);
            }
            else if (token.Type == TokenType.Minus)
            {
                Eat(TokenType.Minus);
            }

            node = new BinOp<T>(node, token, Term());
        }

        return node;
    }

    public AST<T> Term()
    {
        AST<T> node = Factor();

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

            node = new BinOp<T>(node, token, Factor());
        }

        return node;
    }

    public AST<T> Factor()
    {
        Token token = CurrentToken;

        if (token.Type == TokenType.Number)
        {
            Eat(TokenType.Number);
            return new Num<T>(token);
        }
        else if (token.Type == TokenType.LeftParenthesis)
        {
            Eat(TokenType.LeftParenthesis);
            AST<T> node = Expression();
            Eat(TokenType.RightParenthesis);
            return node;
        }

        throw new Exception("Syntax Error");
    }

    public AST<T> Parse()
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
