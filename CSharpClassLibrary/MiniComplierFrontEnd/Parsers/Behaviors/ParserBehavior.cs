using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;
using System;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behaviors;

public abstract class ParserBehavior<T1, T2, T3> : IParserBehavior<T1, T2, T3>
    where T1 : IEnvironment
    where T2 : IStatement
    where T3 : IExpression
{
    private readonly ILexer _lexer;
    private readonly Emitter _emitter;
    public List<Token> MatchedTokens { get; private set; } = new(); 
    public Token? LookAheadToken { get; private set; }
    public T1? TopSymbol { get; private set; }
    public int Used { get; private set; }
    protected ParserBehavior(ILexer lexer, Emitter emitterNode)
    {
        _lexer = lexer;
        _emitter = emitterNode;
    }
    public virtual void Parse(ReadOnlyMemory<char> characters)
    {
        _lexer.Lex(characters);
    }
    public virtual void Error(string s) => throw new Exception($"near line {_lexer.LexLine}: {s}");
    public virtual void LookAhead() => LookAheadToken = _lexer.Scan();
    public virtual void MatchThenLookAhead(int tag)
    {
        if (LookAheadToken?.Tag == tag)
        {
            MatchedTokens.Add(LookAheadToken);
            LookAhead();
        }
        else
        {
            Error($"Syntax error: expecting {Convert.ToChar(tag)} but found {LookAheadToken?.ToString()}");
        }
    }
    public virtual void Program()
    {
        T2 statement = ParseForBlockStatement();
        int begin = statement.Emitter.NewLabel();
        int after = statement.Emitter.NewLabel();
        statement.Emitter.EmitLabel(begin);
        statement.Generate(begin, after);
        statement.Emitter.EmitLabel(after);
    }
    public virtual void Declare()
    {
        while (LookAheadToken?.Tag == TokenTag.BASIC)
        {
            TypeToken typeToken = ParseForTypeToken();
            var token = LookAheadToken;
            MatchThenLookAhead(TokenTag.ID);
            MatchThenLookAhead(';');
            IdExpression idExpression = new((WordToken)token, typeToken, Used, _emitter);
            TopSymbol.Put(token, idExpression);
            Used += typeToken.Width;
        }
    }
    public TypeToken ParseForTypeToken()
    {
        TypeToken typeToken = (TypeToken)LookAheadToken;
        MatchThenLookAhead(TokenTag.BASIC);
        if (LookAheadToken.Tag != '[')
        {
            return typeToken;
        }
        else
        {
            return ParseForDimsToken(typeToken);
        }
    }
    public TypeToken ParseForDimsToken(TypeToken typeToken)
    {
        MatchThenLookAhead('[');
        Token? token = LookAheadToken;
        MatchThenLookAhead(TokenTag.NUMBER);
        MatchThenLookAhead(']');
        if (LookAheadToken?.Tag == '[')
        {
            typeToken = ParseForDimsToken(typeToken);
        }
        return new ArrayTypeToken(typeToken, ((NumberToken)token).Value);
    }
    public T2 ParseForBlockStatement()
    {
        MatchThenLookAhead('{');
        T1 SavedEnvironment = TopSymbol;
        TopSymbol = (T1)Activator.CreateInstance(typeof(T1), new object[] { TopSymbol });
        Declare();
        T2 statement = ParseForStatements();
        MatchThenLookAhead('}');
        TopSymbol = SavedEnvironment;
        return statement;
    }
    public T2 ParseForStatements()
    {
        if (LookAheadToken?.Tag == '}')
        {
            return (T2)Statement.NullStatement;
        }
        else
        {
            return (T2)Activator.CreateInstance(typeof(SequenceStatement), new object[] { ParseForStatement(), ParseForStatements(), _emitter });
        }
    }
    public T2 ParseForStatement()
    {
        IExpression BooleanExpression;
        IStatement statement1, statement2;
        IStatement savedStatement;
        switch (LookAheadToken?.Tag)
        {
            case ';':
                LookAhead();
                return (T2)Statement.NullStatement;
            case TokenTag.IF:
                MatchThenLookAhead(TokenTag.IF);
                MatchThenLookAhead('(');
                BooleanExpression = ParseForBooleanExpression();
                MatchThenLookAhead(')');
                statement1 = ParseForStatement();
                if (LookAheadToken?.Tag != TokenTag.ELSE)
                {
                    return (T2)Activator.CreateInstance(typeof(IfStatement), new object[] { BooleanExpression, statement1, _emitter });
                }
                MatchThenLookAhead(TokenTag.ELSE);
                statement2 = ParseForStatement();
                return (T2)Activator.CreateInstance(typeof(ElseStatement), new object[] { BooleanExpression, statement1, statement2, _emitter });
            case TokenTag.WHILE:
                T2 whileStatment = (T2)Activator.CreateInstance(typeof(WhileStatement), new object[] { _emitter });
                savedStatement = Statement.EnclosingStatement;//need to decouple this
                Statement.EnclosingStatement = whileStatment;
                MatchThenLookAhead(TokenTag.WHILE);
                MatchThenLookAhead('(');
                BooleanExpression = ParseForBooleanExpression();
                MatchThenLookAhead(')');
                statement1 = ParseForStatement();
                whileStatment.Init(BooleanExpression, statement1);
                Statement.EnclosingStatement = savedStatement;
                return whileStatment;
            case TokenTag.DO:
                T2 doStatment = (T2)Activator.CreateInstance(typeof(DoStatement), new object[] { _emitter });
                savedStatement = Statement.EnclosingStatement;//need to decouple this
                Statement.EnclosingStatement = doStatment;
                MatchThenLookAhead(TokenTag.DO);
                statement1 = ParseForStatement();
                MatchThenLookAhead(TokenTag.WHILE);
                MatchThenLookAhead('(');
                BooleanExpression = ParseForBooleanExpression();
                MatchThenLookAhead(')');
                MatchThenLookAhead(';');
                doStatment.Init(BooleanExpression, statement1);
                Statement.EnclosingStatement = savedStatement;
                return doStatment;
            case TokenTag.BREAK:
                MatchThenLookAhead(TokenTag.BREAK);
                MatchThenLookAhead(';');
                return (T2)Activator.CreateInstance(typeof(BreakStatement), new object[] { _emitter });
            case '{':
                return ParseForBlockStatement();
            default:
                return ParseForAssignmentStatement();
        }
    }
    public T2 ParseForAssignmentStatement()
    {
        T2 statement = default;
        Token token = LookAheadToken;
        MatchThenLookAhead(TokenTag.ID);
        IdExpression idExpression = TopSymbol.Get(token);
        if (idExpression is null)
        {
            Error($"{token} undeclared");
        }
        if (LookAheadToken?.Tag == '=')
        {
            LookAhead();
            statement = (T2)Activator.CreateInstance(typeof(SetStatement), new object[] { idExpression, ParseForBooleanExpression(), _emitter });
        }
        else
        {
            AccessingOperationExpression accessingOperationExpression = ParseForOffsetExpression(idExpression);
            MatchThenLookAhead('=');
            statement = (T2)Activator.CreateInstance(typeof(SetElementStatement), new object[] { accessingOperationExpression, ParseForBooleanExpression(), _emitter });
        }
        MatchThenLookAhead(';');
        return statement;
    }
    public T3 ParseForBooleanExpression()
    {
        T3 expression = ParseForJoinExpression();
        while (LookAheadToken?.Tag == TokenTag.OR)
        {
            Token token = LookAheadToken;
            LookAhead();
            expression = (T3)Activator.CreateInstance(typeof(OrExpression), new object[] { token, expression, ParseForJoinExpression(), _emitter });
        }
        return expression;
    }
    public T3 ParseForJoinExpression()
    {
        T3 expression = ParseForEqualityExpression();
        while (LookAheadToken?.Tag == TokenTag.AND)
        {
            Token token = LookAheadToken;
            LookAhead();
            expression = (T3)Activator.CreateInstance(typeof(AndExpression), new object[] { token, expression, ParseForEqualityExpression(), _emitter });
        }
        return expression;
    }
    public T3 ParseForEqualityExpression()
    {
        T3 expression = ParseForRelationExpression();
        while (LookAheadToken?.Tag == TokenTag.EQUAL || LookAheadToken?.Tag == TokenTag.NOT_EQUAL)
        {
            Token token = LookAheadToken;
            LookAhead();
            expression = (T3)Activator.CreateInstance(typeof(RelationExpression), new object[] { token, expression, ParseForRelationExpression(), _emitter });
        }
        return expression;
    }
    public T3 ParseForRelationExpression()
    {
        T3 expression = ParseForArithmeticOperationExpression();
        switch (LookAheadToken?.Tag)
        {
            case TokenTag.LESS_THAN or TokenTag.LESS_OR_EQUAL or TokenTag.GREATER_OR_EQUAL or TokenTag.GREATER_THAN:
                Token token = LookAheadToken;
                LookAhead();
                return (T3)Activator.CreateInstance(typeof(RelationExpression), new object[] { token, expression, ParseForArithmeticOperationExpression(), _emitter });
            default:
                return expression;
        }
    }
    public T3 ParseForArithmeticOperationExpression()
    {
        T3 expression = ParseForTermExpression();
        while (LookAheadToken?.Tag is '+' or '-')
        {
            Token token = LookAheadToken;
            LookAhead();
            expression = (T3)Activator.CreateInstance(typeof(ArithmeticOperationExpression), new object[] { token, expression, ParseForTermExpression(), _emitter });
        }
        return expression;
    }
    public T3 ParseForTermExpression()
    {
        T3 expression = ParseForUnaryExpression();
        while (LookAheadToken?.Tag is '*'
            or '/')
        {
            Token token = LookAheadToken;
            LookAhead();
            expression = (T3)Activator.CreateInstance(typeof(ArithmeticOperationExpression), new object[] { token, expression, ParseForUnaryExpression(), _emitter });
        }
        return expression;
    }
    public T3 ParseForUnaryExpression()
    {
        switch (LookAheadToken?.Tag)
        {
            case '-':
                LookAhead();
                return (T3)Activator.CreateInstance(typeof(UnaryOperationExpression), new object[] { WordToken.MINUS, ParseForUnaryExpression(), _emitter });
            case '!':
                Token token = LookAheadToken;
                LookAhead();
                return (T3)Activator.CreateInstance(typeof(NotExpression), new object[] { token, ParseForUnaryExpression(), _emitter });
            default:
                return ParseForFactorExpression();
        }
    }
    public T3 ParseForFactorExpression()
    {
        T3 expression = default;
        switch (LookAheadToken?.Tag)
        {
            case '(':
                LookAhead();
                expression = ParseForBooleanExpression();
                MatchThenLookAhead(')');
                return expression;
            case TokenTag.NUMBER:
                expression = (T3)Activator.CreateInstance(typeof(ConstantExpression), new object[] { LookAheadToken, TypeToken.INT, _emitter });
                LookAhead();
                return expression;
            case TokenTag.REAL:
                expression = (T3)Activator.CreateInstance(typeof(ConstantExpression), new object[] { LookAheadToken, TypeToken.FLOAT, _emitter });
                LookAhead();
                return expression;
            case TokenTag.TRUE:
                expression = (T3)Activator.CreateInstance(typeof(ConstantExpression), new object[] { WordToken.TRUE, TypeToken.BOOL, _emitter });
                LookAhead();
                return expression;
            case TokenTag.FALSE:
                expression = (T3)Activator.CreateInstance(typeof(ConstantExpression), new object[] { WordToken.FALSE, TypeToken.BOOL, _emitter });
                LookAhead();
                return expression;
            case TokenTag.ID:
                var idExpression = TopSymbol.Get(LookAheadToken);
                if (idExpression == null)
                {
                    Error($"{LookAheadToken} undeclared");
                }
                LookAhead();
                if (LookAheadToken?.Tag != '[')
                {
                    return (T3)Activator.CreateInstance(typeof(IdExpression), new object[] {
                        idExpression.Token,
                        idExpression.TypeToken,
                        idExpression.Offset,
                        idExpression.Node
                    });
                }
                else
                {
                    var accessingOperationExpression = ParseForOffsetExpression(idExpression);
                    return (T3)Activator.CreateInstance(typeof(AccessingOperationExpression), new object[] {
                        accessingOperationExpression.ArrayExpression,
                        accessingOperationExpression.IndexExpression,
                        accessingOperationExpression.TypeToken,
                        accessingOperationExpression.Node
                    });
                }
            default:
                Error("Syntax error");
                return expression;
        }
    }
    public AccessingOperationExpression ParseForOffsetExpression(IdExpression idExpression)
    {
        IExpression booleanExpression;
        IExpression constantExpression;
        IExpression arithmeticOperationExpression1;
        IExpression arithmeticOperationExpression2;
        IExpression loc;
        TypeToken typeToken = idExpression.TypeToken;
        MatchThenLookAhead('[');
        booleanExpression = ParseForBooleanExpression();
        MatchThenLookAhead(']');
        typeToken = ((ArrayTypeToken)typeToken).OfTypeToken;
        constantExpression = new ConstantExpression(typeToken.Width, _emitter);
        arithmeticOperationExpression1 = new ArithmeticOperationExpression(new Token('*'), booleanExpression, constantExpression, _emitter);
        loc = arithmeticOperationExpression1;
        while (LookAheadToken?.Tag == '[')
        {
            MatchThenLookAhead('[');
            booleanExpression = ParseForBooleanExpression();
            MatchThenLookAhead(']');
            typeToken = ((ArrayTypeToken)typeToken).OfTypeToken;
            constantExpression = new ConstantExpression(typeToken.Width, _emitter);
            arithmeticOperationExpression1 = new ArithmeticOperationExpression(new Token('*'), booleanExpression, constantExpression, _emitter);
            arithmeticOperationExpression2 = new ArithmeticOperationExpression(new Token('+'), loc, arithmeticOperationExpression1, _emitter);
            loc = arithmeticOperationExpression2;
        }
        return new AccessingOperationExpression(idExpression, loc, typeToken, _emitter);
    }
}
