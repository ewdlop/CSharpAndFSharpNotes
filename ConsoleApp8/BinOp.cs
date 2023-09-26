using System.Numerics;

namespace ConsoleApp8;

public record BinOp<T>(AST<T> Left, Token Op, AST<T> Right) : AST<T> 
    where T: INumber<T>
{
    public override T Accept(IASTVisitor<T> visitor)
    {
        return visitor.VisitBinOp(this);
    }
}
