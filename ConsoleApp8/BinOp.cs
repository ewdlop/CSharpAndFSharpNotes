namespace ConsoleApp8;

public record BinOp(AST Left, Token Op, AST Right) : AST
{
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.VisitBinOp(this);
    }
}
