﻿namespace LLVMApp.AST;

public record NumberExpressionAST(double Value) : ExpressionAST(ExpressionType.NumberExpression)
{
    public override ExpressionAST Accept(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.VisitNumberExpressionAST(this);
    }
}