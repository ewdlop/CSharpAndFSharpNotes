﻿namespace LLVMApp.AST;

public record PrototypeExpressionAST(string Name, IEnumerable<string> Arguments) 
    : ExpressionAST(ExpressionType.PrototypeExpression)
{
    public override ExpressionAST Accept(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.VisitPrototypeExpressionAST(this);
    }
}