namespace LLVMApp.AST;

public record IfExpressionAST(ExpressionAST Condition, ExpressionAST Then, ExpressionAST Else) 
    : ExpressionAST(ExpressionType.IfExpression)
{
    public override ExpressionAST Accept(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.VisitIfExpressionAST(this);
    }
}