﻿using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record LogicalExpression(Token Token, IExpression? Expression1, IExpression? Expression2, ILabelEmitter Node, TypeToken? TypeToken = null)
    : Expression(Token, TypeToken??Check(Expression1?.TypeToken, Expression2?.TypeToken), Node)
{
    public static TypeToken Check(TypeToken? typeToken1, TypeToken? typeToken2)
    {
        if (typeToken1 == TypeToken.BOOL && typeToken2 == TypeToken.BOOL)
        {
            return TypeToken.BOOL;
        }
        else
        {
            throw new System.Exception("type error");
        }
    }

    public override Expression Generate()
    {
        int f = Node.NewLabel();
        int a = Node.NewLabel();
        var temporaryExpressionNode = new TemporaryExpression(TypeToken, Node);
        Jumping(0, f);
        Node.Emit($"{temporaryExpressionNode} = true");
        Node.Emit($"goto L{a}");
        Node.EmitLabel(f);
        Node.Emit($"{temporaryExpressionNode} = false");
        Node.EmitLabel(a);
        return temporaryExpressionNode;
    }
    public override string ToString() => base.ToString();

}
