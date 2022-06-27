namespace LLVMApp.AST;

public abstract record ExpressionAST(ExpressionType NodeType)
{
    public virtual ExpressionAST VisitChildren(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }

    public virtual ExpressionAST Accept(ExpressionVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);   
        return visitor.VisitExtension(this);
    }
}