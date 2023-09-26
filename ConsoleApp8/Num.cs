using System.Numerics;

namespace ConsoleApp8;

public record Num<T>(Token Value) : AST<T> where T : INumber<T>
{
    public override T Accept(IASTVisitor<T> visitor)
    {
        return visitor.VisitNum(this);
    }
}