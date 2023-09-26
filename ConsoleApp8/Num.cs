namespace ConsoleApp8;

public record Num(Token Value) : AST
{
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.VisitNum(this);
    }
}