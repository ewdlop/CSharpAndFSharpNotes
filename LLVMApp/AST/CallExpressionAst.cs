namespace LLVMApp.AST;

public record CallExpressionAst(string? Calle, IEnumerable<ExpressionAST>? Arguments)
    : ExpressionAST(ExpressionType.CallExpression)
{
    public override ExpressionAST? Accept(ExpressionVisitor? visitor)
    {
        return visitor?.VisitCallExpressionAST(this);
    }
}