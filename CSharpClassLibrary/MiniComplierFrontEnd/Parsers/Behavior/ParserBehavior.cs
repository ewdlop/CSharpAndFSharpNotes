using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behavior;

public abstract class ParserBehavior<T1,T2,T3> : IParserBehavior<T1, T2,T3>
    where T1: IEnvironment
    where T2: IStatement
    where T3 : IExpression
{
    private readonly ILexer _lexer;
    private readonly Node _emitterNode;
    public Token LookAheadToken { get; private set; }
    public T1 TopSymbol { get; private set; }
    public int Used { get; private set; }
    protected ParserBehavior(ILexer lexer, Node emitterNode)
    {
        _lexer = lexer;
        _emitterNode = emitterNode;
        Move();
    }
    public virtual void Error(string s) => throw new Exception($"near line {_lexer.LexLine}: {s}");
    public virtual void Move() => LookAheadToken = _lexer.Scan();
    public virtual void Match(int tag)
    {
        if (LookAheadToken.Tag == tag)
        {
            Move();
        }
        else
        {
            Error("Syntax error");
        }
    }
    public virtual void Program()
    {
        var statement = Block();
        int begin = statement.EmitterNode.NewLabel();
        int after = statement.EmitterNode.NewLabel();
        statement.EmitterNode.EmitLabel(begin);
        statement.Generate(begin,after);
        statement.EmitterNode.EmitLabel(after);
    }
    public virtual void Declare()
    {
        while (LookAheadToken.Tag == TokenTag.BASIC)
        {
            TypeToken typeToken = Type();
            var token = LookAheadToken;
            Match(TokenTag.ID);
            Match(';');
            IdExpression idExpression = new((WordToken)token, typeToken, Used, _emitterNode);
            TopSymbol.Put(token, idExpression);
            Used += typeToken.Width;
        }
    }
    public TypeToken Type()
    {
        TypeToken typeToken = (TypeToken)LookAheadToken;
        Match(TokenTag.BASIC);
        if(LookAheadToken.Tag != '[')
        {
            return typeToken;
        }
        else
        {
            return Dims(typeToken);
        }
    }
    public TypeToken Dims(TypeToken typeToken)
    {
        Match('[');
        Token token = LookAheadToken;
        Match(TokenTag.NUMBER);
        Match(']');
        if(LookAheadToken.Tag =='[')
        {
            typeToken = Dims(typeToken);
        }
        return new ArrayTypeToken(typeToken,((NumberToken)token).Value);
    }
    public T2 Block()
    {
        Match('{');
        T1 SavedEnvironment = TopSymbol;
        TopSymbol = (T1)Activator.CreateInstance(typeof(T1), new object[] { TopSymbol });
        Declare();
        T2 statement = Statements();
        Match('}');
        TopSymbol = SavedEnvironment;
        return statement;
    }
    public T2 Statements()
    {
        if(LookAheadToken.Tag == '}')
        {
            return (T2)Intermediate.Statements.Statement.NullStatement;
        }
        else
        {
            return (T2)Activator.CreateInstance(typeof(SequenceStatement), new object[] { Statement(), Statements(), _emitterNode });
        }
    }
    public T2 Statement()
    {
        IExpression BooleanExpression;
        IStatement statement1, statement2;
        IStatement savedStatement;
        switch (LookAheadToken.Tag)
        {
            case ';':
                Move();
                return (T2)Intermediate.Statements.Statement.NullStatement;
            case TokenTag.IF:
                Match(TokenTag.IF);
                Match('(');
                BooleanExpression = Boolean();
                Match(')');
                statement1 = Statement();
                if(LookAheadToken.Tag !=TokenTag.ELSE)
                {
                    return (T2)Activator.CreateInstance(typeof(IfStatement), new object[] { BooleanExpression, statement1, _emitterNode });
                }
                Match(TokenTag.ELSE);
                statement2 = Statement();
                return (T2)Activator.CreateInstance(typeof(ElseStatement), new object[] { BooleanExpression, statement1, statement2, _emitterNode });
            case TokenTag.WHILE:
                T2 whileStatment = (T2)Activator.CreateInstance(typeof(WhileStatement), new object[] { _emitterNode });
                savedStatement = Intermediate.Statements.Statement.EnclosingStatement;//need to decouple this
                Intermediate.Statements.Statement.EnclosingStatement = whileStatment;
                Match(TokenTag.WHILE);
                Match('(');
                BooleanExpression = Boolean();
                Match(')');
                statement1 = Statement();
                whileStatment.Init(BooleanExpression, statement1);
                Intermediate.Statements.Statement.EnclosingStatement = savedStatement;
                return whileStatment;
            case TokenTag.DO:
                T2 doStatment = (T2)Activator.CreateInstance(typeof(DoStatement));
                savedStatement = Intermediate.Statements.Statement.EnclosingStatement;//need to decouple this
                Intermediate.Statements.Statement.EnclosingStatement = doStatment;
                Match(TokenTag.DO);
                statement1 = new Statement(_emitterNode);
                Match(TokenTag.WHILE);
                Match('(');
                BooleanExpression = Boolean();
                Match(')');
                Match(';');
                doStatment.Init(BooleanExpression, statement1);
                Intermediate.Statements.Statement.EnclosingStatement = savedStatement;
                return doStatment;
            case TokenTag.BREAK:
                Match(TokenTag.BREAK);
                Match(';');
                return (T2)Activator.CreateInstance(typeof(BreakStatement));
            case '{':
                return Block();
            default:
                return Assign();
        }
    }
    public T2 Assign()
    {
        Token token = LookAheadToken;
        Match(TokenTag.ID);
        IdExpression idExpression = TopSymbol.Get(token);
        if(idExpression == null)
        {
            _emitterNode.Error($"{token} undeclared");
        }
        if (LookAheadToken.Tag == '=')
        {
            Move();
            return (T2)Activator.CreateInstance(typeof(SetStatement), new object[] { idExpression, Boolean(), _emitterNode });
        }
        else
        {
            AccessingOperationExpression accessingOperationExpression = Offset(idExpression);
            Match('=');
            return (T2)Activator.CreateInstance(typeof(SetElementStatement), new object[] { accessingOperationExpression, Boolean(), _emitterNode });
        }
    }
    public T3 Boolean()
    {
        T3 expression = Join();
        while(LookAheadToken.Tag == TokenTag.OR)
        {
            Token token = LookAheadToken;
            Move();
            expression = (T3)Activator.CreateInstance(typeof(OrExpression), new object[] { token, expression, Join(), _emitterNode });
        }
        return expression;
    }
    public T3 Join()
    {
        T3 expression = Equality();
        while(LookAheadToken.Tag == TokenTag.AND)
        {
            Token token = LookAheadToken;
            Move();
            expression = (T3)Activator.CreateInstance(typeof(AndExpression), new object[] { token, expression, Equality(), _emitterNode });
        }
        return expression;
    }
    public T3 Equality()
    {
        T3 expression = Relation();
        while (LookAheadToken.Tag == TokenTag.EQUAL || LookAheadToken.Tag == TokenTag.NOT_EQUAL)
        {
            Token token = LookAheadToken;
            Move();
            expression = (T3)Activator.CreateInstance(typeof(RelationExpression), new object[] { token, expression, Relation(), _emitterNode });
        }
        return expression;
    }
    public T3 Relation()
    {
        T3 expression = Expression();
        switch(LookAheadToken.Tag)
        {
            case '<':
                Token token = LookAheadToken;
                Move();
                return (T3)Activator.CreateInstance(typeof(RelationExpression), new object[] { token,expression,Expression(), _emitterNode });
            default:
                return expression;
        }
    }
    public T3 Expression()
    {
        T3 expression = Term();
        while (LookAheadToken.Tag is '+' or '-')
        {
            Token token = LookAheadToken;
            Move();
            expression = (T3)Activator.CreateInstance(typeof(ArithmeticOperationExpression), new object[] { token, expression, Term(), _emitterNode });
        }
        return expression;
    }
    public T3 Term()
    {
        T3 expression = Unary();
        while (LookAheadToken.Tag is '*'
            or '/')
        {
            Token token = LookAheadToken;
            Move();
            expression = (T3)Activator.CreateInstance(typeof(ArithmeticOperationExpression), new object[] { token, expression, Unary(), _emitterNode });
        }
        return expression;
    }
    public T3 Unary()
    {
        switch (LookAheadToken.Tag)
        {
            case '-':
                Move();
                return (T3)Activator.CreateInstance(typeof(UnaryOperationExpression), new object[] { WordToken.MINUS, Unary(), _emitterNode });
            case '!':
                Token token = LookAheadToken;
                Move();
                return (T3)Activator.CreateInstance(typeof(NotExpression), new object[] { token, Unary(), _emitterNode });
            default:
                return Factor();
        }
    }
    public T3 Factor()
    {
        T3 expression = default;
        switch(LookAheadToken.Tag)
        {
            case '(':
                Move();
                expression = Boolean();
                Match(')');
                return expression;
            case TokenTag.NUMBER:
                expression = (T3)Activator.CreateInstance(typeof(ConstantExpression), new object[] { TypeToken.INT, _emitterNode });
                Move();
                return expression;
            case TokenTag.REAL:
                expression = (T3)Activator.CreateInstance(typeof(ConstantExpression), new object[] { TypeToken.FLOAT, _emitterNode });
                Move();
                return expression;
            case TokenTag.TRUE:
                expression = (T3)Activator.CreateInstance(typeof(ConstantExpression), new object[] { WordToken.TRUE, TypeToken.BOOL, _emitterNode });
                Move();
                return expression;
            case TokenTag.FALSE:
                expression= (T3)Activator.CreateInstance(typeof(ConstantExpression), new object[] { WordToken.FALSE, TypeToken.BOOL, _emitterNode });
                Move();
                return expression;
            case TokenTag.ID:
                var idExpression = TopSymbol.Get(LookAheadToken);
                if(idExpression == null)
                {
                    _emitterNode.Error($"{LookAheadToken} undeclared");
                }
                Move();
                if(LookAheadToken.Tag != '[')
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
                    var accessingOperationExpression = Offset(idExpression);
                    return (T3)Activator.CreateInstance(typeof(AccessingOperationExpression), new object[] { 
                        accessingOperationExpression.ArrayExpression, 
                        accessingOperationExpression.IndexExpression, 
                        accessingOperationExpression.TypeToken, 
                        accessingOperationExpression.Node 
                    });
                }
            default:
                _emitterNode.Error("Syntax error");
                return expression;
        }
    }
    public AccessingOperationExpression Offset(IdExpression idExpression)
    {
        IExpression booleanExpression;
        IExpression constantExpression;
        IExpression arithmeticOperationExpression1;
        IExpression arithmeticOperationExpression2;
        IExpression loc;
        TypeToken typeToken = idExpression.TypeToken;
        Match('[');
        booleanExpression = Boolean();
        Match(']');
        typeToken = ((ArrayTypeToken)typeToken).OfTypeToken;
        constantExpression = new ConstantExpression(typeToken.Width, _emitterNode);
        arithmeticOperationExpression1 = new ArithmeticOperationExpression(new Token('*'), booleanExpression, constantExpression, _emitterNode);
        loc = arithmeticOperationExpression1;
        while(LookAheadToken.Tag == '[')
        {
            Match('[');
            booleanExpression = Boolean();
            Match(']');
            typeToken = ((ArrayTypeToken)typeToken).OfTypeToken;
            constantExpression = new ConstantExpression(typeToken.Width, _emitterNode);
            arithmeticOperationExpression1 = new ArithmeticOperationExpression(new Token('*'), booleanExpression, constantExpression, _emitterNode);
            arithmeticOperationExpression2 = new ArithmeticOperationExpression(new Token('+'), loc, arithmeticOperationExpression1, _emitterNode);
            loc = arithmeticOperationExpression2;
        }
        return new AccessingOperationExpression(idExpression, loc, typeToken,_emitterNode);
    }
}
