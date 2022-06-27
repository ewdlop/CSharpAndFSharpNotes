namespace LLVMApp.AST;

public record FunctionExpressionAST(PrototypeExpressionAST Prototype, ExpressionAST Body)
    : ExpressionAST(ExpressionType.FunctionExpression)
{
    public override ExpressionAST Accept(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.VisitFunctionAST(this);
    }
}