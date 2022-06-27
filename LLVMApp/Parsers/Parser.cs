namespace LLVMApp.Parsers;

using System;
using LLVMApp.AST;
using LLVMApp.Lexers;

public class Parser : IParser
{

    private readonly ILexer _scanner;

    private readonly BaseParserListener _baseListener;

    public Parser(ILexer scanner, IParserListener parserListener)
    {
        _scanner = scanner;
        _baseListener = new BaseParserListener(parserListener);
    }

    public void HandleDefinition()
    {
        _baseListener.EnterRule("HandleDefinition");
        FunctionExpressionAST? functionAST = ParseDefinition();
        _baseListener.ExitRule(functionAST);
        if (functionAST is not null)
        {
            _baseListener.Listen();
        }
        else
        {
            // Skip token for error recovery.
            _scanner.GetNextToken();
        }
    }

    public void HandleExtern()
    {
        _baseListener.EnterRule("HandleExtern");

        PrototypeExpressionAST? prototypeAST = ParseExtern();

        _baseListener.ExitRule(prototypeAST);

        if (prototypeAST is not null)
        {
            _baseListener.Listen();
        }
        else
        {
            // Skip token for error recovery.
            _scanner.GetNextToken();
        }
    }

    public void HandleTopLevelExpression()
    {
        _baseListener.EnterRule("HandleTopLevelExpression");

        var functionAST = ParseTopLevelExpression();

        _baseListener.ExitRule(functionAST);

        if (functionAST is not null)
        {
            _baseListener.Listen();
        }
        else
        {
            // Skip token for error recovery.
            _scanner.GetNextToken();
        }
    }
    private ExpressionAST? ParseIdentifierExpression()
    {
        string? idName = _scanner.LastIdentifier;

        _scanner.GetNextToken();  // eat identifier.

        if (_scanner.CurrentToken != '(') // Simple variable ref.
        {
            return new VariableExpressionAST(idName);
        }

        // Call.
        _scanner.GetNextToken();  // eat (
        List<ExpressionAST> args = new();

        if (_scanner.CurrentToken != ')')
        {
            while (true)
            {
                ExpressionAST? arg = ParseExpression();
                if (arg is null)
                {
                    return null;
                }

                args.Add(arg);

                if (_scanner.CurrentToken == ')')
                {
                    break;
                }

                if (_scanner.CurrentToken != ',')
                {
                    Console.WriteLine("Expected ')' or ',' in argument list");
                    return null;
                }

                _scanner.GetNextToken();
            }
        }

        // Eat the ')'.
        _scanner.GetNextToken();

        return new CallExpressionAst(idName, args);
    }

    private ExpressionAST? ParseNumberExpression()
    {
        ExpressionAST result = new NumberExpressionAST(_scanner.LastNumberValue);
        _scanner.GetNextToken();
        return result;
    }
    private ExpressionAST? ParseParensisExpression()
    {
        _scanner.GetNextToken();  // eat (.
        ExpressionAST? expression = ParseExpression();
        if (expression is null)
        {
            return null;
        }

        if (_scanner.CurrentToken != ')')
        {
            Console.WriteLine("expected ')'");
            return null;
        }

        _scanner.GetNextToken(); // eat ).

        return expression;
    }

    private ExpressionAST? ParseExpression()
    {
        ExpressionAST? lhs = ParsePrimary();
        if (lhs is null)
        {
            return null;
        }

        return ParseBinaryOperationRHS(0, ref lhs);
    }

    public ExpressionAST? ParseIfExpression()
    {
        _scanner.GetNextToken(); // eat the if.

        // condition
        ExpressionAST? conditionExpression = ParseExpression();
        if (conditionExpression is null)
        {
            return null;
        }

        if (_scanner.CurrentToken != (int)Token.THEN)
        {
            Console.WriteLine("expected then");
        }

        _scanner.GetNextToken(); // eat the then

        ExpressionAST? thenExpression = ParseExpression();
        if (thenExpression is null)
        {
            return null;
        }

        if (_scanner.CurrentToken != (int)Token.ELSE)
        {
            Console.WriteLine("expected else");
            return null;
        }

        _scanner.GetNextToken();

        ExpressionAST? elseExpression = ParseExpression();
        if (elseExpression is null)
        {
            return null;
        }

        return new IfExpressionAST(conditionExpression, thenExpression, elseExpression);
    }

    public ExpressionAST? ParseForExpression()
    {
        _scanner.GetNextToken(); // eat the for.

        if (_scanner.CurrentToken != (int)Token.IDENTIFIER)
        {
            Console.WriteLine("expected identifier after for");
            return null;
        }

        string? idName = _scanner.LastIdentifier;
        _scanner.GetNextToken(); // eat identifier.

        if (_scanner.CurrentToken != '=')
        {
            Console.WriteLine("expected '=' after for");
            return null;
        }

        _scanner.GetNextToken(); // eat '='.

        ExpressionAST? startExpression = ParseExpression();
        if (startExpression is null)
        {
            return null;
        }

        if (_scanner.CurrentToken != ',')
        {
            Console.WriteLine("expected ',' after for start value");
            return null;
        }

        _scanner.GetNextToken();

        ExpressionAST? endExpression = ParseExpression();
        if (endExpression is null)
        {
            return null;
        }

        // The step value is optional;
        ExpressionAST? stepExpression = null;
        if (_scanner.CurrentToken == ',')
        {
            _scanner.GetNextToken();
            stepExpression = ParseExpression();
            if (stepExpression is null)
            {
                return null;
            }
        }

        if (_scanner.CurrentToken != (int)Token.IN)
        {
            Console.WriteLine("expected 'in' after for");
            return null;
        }

        _scanner.GetNextToken();
        ExpressionAST? bodyExpression = ParseExpression();
        if (bodyExpression is null)
        {
            return null;
        }

        return new ForExpressionAST(idName, startExpression, endExpression, stepExpression, bodyExpression);
    }


    private ExpressionAST? ParseBinaryOperationRHS(int? expressionPrecedence, ref ExpressionAST lhs)
    {
        while (true)
        {
            int? tokenPrecedence = _scanner.GetTokenPrecedence();
            // If this is a binop that binds at least as tightly as the current binop,
            // consume it, otherwise we are done.
            if (tokenPrecedence < expressionPrecedence)
            {
                return lhs;
            }
            // Okay, we know this is a binop.
            int? binaryOperation = _scanner.CurrentToken;
            _scanner.GetNextToken();  // eat binop

            // Parse the primary expression after the binary operator.
            ExpressionAST? rhs = ParsePrimary();
            if (rhs is null)
            {
                return null;
            }

            // If BinOp binds less tightly with RHS than the operator after RHS, let
            // the pending operator take RHS as its LHS.
            int? nextPrecedence = _scanner.GetTokenPrecedence();
            if (tokenPrecedence < nextPrecedence)
            {
                rhs = ParseBinaryOperationRHS(tokenPrecedence + 1, ref rhs);
                if (rhs == null)
                {
                    return null;
                }
            }

            // Merge LHS/RHS.
            lhs = new BinaryExpressionAST((char)binaryOperation, lhs, rhs);
        }
    }

    private ExpressionAST? ParsePrimary() => _scanner.CurrentToken switch
    {
        (int) Token.IDENTIFIER => ParseIdentifierExpression(),
        (int) Token.NUMBER => ParseNumberExpression(),
        '(' => ParseParensisExpression(),
        (int) Token.IF => ParseIfExpression(),
        (int) Token.FOR => ParseForExpression(),
        _ => null
    };


    private PrototypeExpressionAST? ParsePrototype()
    {
        if (_scanner.CurrentToken != (int)Token.IDENTIFIER)
        {
            Console.WriteLine("Expected function name in prototype");
            return null;
        }

        string? functionName = _scanner.LastIdentifier;

        _scanner.GetNextToken();

        if (_scanner.CurrentToken != '(')
        {
            Console.WriteLine("Expected '(' in prototype");
            return null;
        }

        List<string> argumentNames = new();
        while (_scanner.GetNextToken() == (int)Token.IDENTIFIER)
        {
            argumentNames.Add(_scanner.LastIdentifier);
        }

        if (_scanner.CurrentToken != ')')
        {
            Console.WriteLine("Expected ')' in prototype");
            return null;
        }

        _scanner.GetNextToken(); // eat ')'.

        return new PrototypeExpressionAST(functionName, argumentNames);
    }
    private FunctionExpressionAST? ParseDefinition()
    {
        _scanner?.GetNextToken();
        PrototypeExpressionAST? proto = ParsePrototype();

        if (proto is null)
        {
            return null;
        }

        ExpressionAST? body = ParseExpression();
        if (body is null)
        {
            return null;
        }

        return new FunctionExpressionAST(proto, body);
    }
    private FunctionExpressionAST? ParseTopLevelExpression()
    {
        ExpressionAST? e = ParseExpression();
        if (e is null)
        {
            return null;
        }

        // Make an anonymous proto.
        PrototypeExpressionAST proto = new(string.Empty, new List<string>());
        return new FunctionExpressionAST(proto, e);
    }
    private PrototypeExpressionAST? ParseExtern()
    {
        _scanner.GetNextToken();  // eat extern.
        return ParsePrototype();
    }
}
