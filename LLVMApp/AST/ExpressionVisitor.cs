namespace LLVMApp.AST;

public abstract record ExpressionVisitor
{
    public virtual ExpressionAST Visit(ExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return node.Accept(this);
    }

    public virtual ExpressionAST VisitExtension(ExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return node.VisitChildren(this);
    }

    public virtual ExpressionAST VisitBinaryExpressionAST(BinaryExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        Visit(node.Lhs);
        Visit(node.Rhs);
        return node;
    }

    public virtual ExpressionAST VisitCallExpressionAST(CallExpressionAst node)
    {
        ArgumentNullException.ThrowIfNull(node);
        foreach (ExpressionAST argument in node.Arguments ?? Enumerable.Empty<ExpressionAST>())
        {
            Visit(argument);
        }
        return node;
    }

    public virtual ExpressionAST VisitFunctionAST(FunctionExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        Visit(node.Prototype);
        Visit(node.Body);
        return node;
    }

    public virtual ExpressionAST VisitVariableExpressionAST(VariableExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return node;
    }

    public virtual ExpressionAST VisitPrototypeExpressionAST(PrototypeExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return node;
    }

    public virtual ExpressionAST VisitNumberExpressionAST(NumberExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return node;
    }

    public virtual ExpressionAST VisitIfExpressionAST(IfExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        Visit(node.Condition);
        Visit(node.Then);
        Visit(node.Else);

        return node;
    }

    public virtual ExpressionAST VisitForExpressionAST(ForExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        Visit(node.Start);
        Visit(node.End);
        Visit(node.Step);
        Visit(node.Body);

        return node;
    }
}