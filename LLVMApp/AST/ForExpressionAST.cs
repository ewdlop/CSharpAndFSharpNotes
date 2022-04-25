namespace LLVMApp.AST;

public record ForExpressionAST(string? VariableName,
                               ExpressionAST? Start,
                               ExpressionAST? End,
                               ExpressionAST? Step,
                               ExpressionAST? Body)
    : ExpressionAST(ExpressionType.FoExpression)
{
    public override ExpressionAST? Accept(ExpressionVisitor? visitor)
    {
        return visitor?.VisitForExpressionAST(this);
    }
}

public record VariableExpressionAST(string? Name) : ExpressionAST(ExpressionType.VariableExpression)
{
    public override ExpressionAST? Accept(ExpressionVisitor? visitor)
    {
        return visitor?.VisitVariableExpressionAST(this);
    }
}