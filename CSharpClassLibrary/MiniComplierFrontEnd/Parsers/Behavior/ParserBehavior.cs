using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behavior;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behavior
{
    public class ParserBehavior<T1,T2,T3> : IParserBehavior<T1, T2,T3>
        where T1: IEnvironment, new()
        where T2: IStatement, new()
        where T3 : IExpression, new()
    {
        private readonly ILexerBehavior _lexerBehavior;
        public Token LookAheadToken { get; }
        public T1 TopSymbol { get; }
        public int Used { get; }
        public ParserBehavior(LexerBehavior lexerBehavior)
        {
            _lexerBehavior = lexerBehavior;
        }
        void IParserBehavior<T1,T2,T3>.Error(string s) => throw new System.Exception($"near line {_lexerBehavior.Line}: {s}");
        public void Move() => _lexerBehavior.Scan();
        public void Match(int tag)
        {
            if (LookAheadToken.Tag == tag)
            {
                Move();
            }
            else
            {
                (this as IParserBehavior<T1,T2,T3>).Error("Syntax error");
            }
        }
        public void Program()
        {

        }
        public void Declare()
        {
            while (LookAheadToken.Tag == TokenTag.BASIC)
            {
                //TypeToken typeToke =
            }
        }
        public TypeToken Type()
        {
            return null;
        }
        public TypeToken Dims()
        {
            return null;
        }
        public T2 Block()
        {
            return default;
        }

        public T2 Statements()
        {
            throw new System.NotImplementedException();
        }

        public T2 Statement()
        {
            throw new System.NotImplementedException();
        }

        public T2 Assign()
        {
            throw new System.NotImplementedException();
        }

        public T3 Bool()
        {
            throw new System.NotImplementedException();
        }

        public T3 Join()
        {
            throw new System.NotImplementedException();
        }

        public T3 Equality()
        {
            throw new System.NotImplementedException();
        }

        public T3 Relation()
        {
            throw new System.NotImplementedException();
        }

        public T3 Expression()
        {
            throw new System.NotImplementedException();
        }

        public T3 Term()
        {
            throw new System.NotImplementedException();
        }

        public T3 Unary()
        {
            throw new System.NotImplementedException();
        }

        public T3 Factor()
        {
            throw new System.NotImplementedException();
        }

        public AccessingOperationExpression Offset(IdExpression id)
        {
            throw new System.NotImplementedException();
        }
    }
}
