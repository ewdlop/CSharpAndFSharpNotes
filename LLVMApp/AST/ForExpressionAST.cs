namespace LLVMApp.AST;

public record ForExpressionAST(string VariableName,
                               ExpressionAST Start,
                               ExpressionAST End,
                               ExpressionAST Step,
                               ExpressionAST Body)
    : ExpressionAST(ExpressionType.FoExpression)
{
    public override ExpressionAST Accept(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.VisitForExpressionAST(this);
    }
}
