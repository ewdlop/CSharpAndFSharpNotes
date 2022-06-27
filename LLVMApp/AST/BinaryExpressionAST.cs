namespace LLVMApp.AST;

public record BinaryExpressionAST(char Operation, ExpressionAST Lhs, ExpressionAST Rhs) 
    : ExpressionAST(Operation switch
    {
        '+' => ExpressionType.AdditionExpression,
        '-' => ExpressionType.SubtractExpression,
        '*' => ExpressionType.MultiplyExpression,
        '<' => ExpressionType.LessThanExpression,
        _ => throw new ArgumentException($"operator {Operation} is not a valid operator")
    })
{
    public override ExpressionAST Accept(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.VisitBinaryExpressionAST(this);
    }
}