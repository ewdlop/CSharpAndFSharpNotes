namespace LLVMApp.AST;

public record IfExpressionAST(ExpressionAST? Condition, ExpressionAST? Then, ExpressionAST? Else) 
    : ExpressionAST(ExpressionType.IfExpression)
{
    public override ExpressionAST? Accept(ExpressionVisitor? visitor)
    {
        return visitor?.VisitIfExpressionAST(this);
    }
}