namespace LLVMApp.AST;

public record VariableExpressionAST(string Name) : ExpressionAST(ExpressionType.VariableExpression)
{
    public override ExpressionAST Accept(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.VisitVariableExpressionAST(this);
    }
}