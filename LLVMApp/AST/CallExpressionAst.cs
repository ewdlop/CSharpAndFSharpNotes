namespace LLVMApp.AST;

public record CallExpressionAst(string Callee, IEnumerable<ExpressionAST> Arguments)
    : ExpressionAST(ExpressionType.CallExpression)
{
    public override ExpressionAST Accept(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.VisitCallExpressionAST(this);
    }
}