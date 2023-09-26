using System.Numerics;

namespace ConsoleApp8;

public interface IASTVisitor<T> where T : INumber<T>
{
    T VisitBinOp(BinOp<T> binOp);
    T VisitNum(Num<T> num);
}

public abstract record AST<T> where T : INumber<T>
{
    public abstract T Accept(IASTVisitor<T> visitor);
}