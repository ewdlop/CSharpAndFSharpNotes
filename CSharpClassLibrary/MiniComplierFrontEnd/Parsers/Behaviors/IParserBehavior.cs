using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;
using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Parsers.Behaviors;

public interface IParserBehavior<T1, T2, T3> : IReadOnlyParserBehavior<T1>
    where T1 : IEnvironment
    where T2 : IStatement
    where T3 : IExpression
{
    void Parse(ReadOnlyMemory<char> enmerable);
    void LookAhead();
    void MatchThenLookAhead(int tag);
    void Error(string s);
    void Program();
    void Declare();
    TypeToken ParseForTypeToken();
    TypeToken ParseForDimsToken(TypeToken typeToken);
    T2 ParseForBlockStatement();
    T2 ParseForStatements();
    T2 ParseForStatement();
    T2 ParseForAssignmentStatement();
    T3 ParseForBooleanExpression();
    T3 ParseForJoinExpression();
    T3 ParseForEqualityExpression();
    T3 ParseForRelationExpression();
    T3 ParseForArithmeticOperationExpression();
    T3 ParseForTermExpression();
    T3 ParseForUnaryExpression();
    T3 ParseForFactorExpression();
    AccessingOperationExpression ParseForOffsetExpression(IdExpression id);
}
