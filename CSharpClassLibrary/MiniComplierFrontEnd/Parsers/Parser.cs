using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behaviors;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using System;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers;

public abstract class Parser<T1,T2,T3> : IParser<T1, T2, T3>
    where T1 : IEnvironment
    where T2 : IStatement
    where T3 : IExpression
{
    private readonly IParserBehavior<T1, T2, T3> _parserBehavior;
    public Parser(IParserBehavior<T1, T2, T3> parserBehavior)
    {
        _parserBehavior = parserBehavior;
    }
    public virtual void Program()
    {
        _parserBehavior.Program();
    }
    public virtual void Parse(ReadOnlyMemory<char> characters) => _parserBehavior.Parse(characters);
    public virtual void Move() => _parserBehavior.LookAhead();
    public virtual void Match(int tag) => _parserBehavior.MatchThenLookAhead(tag);
}
