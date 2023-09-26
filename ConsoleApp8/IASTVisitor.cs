namespace ConsoleApp8;

public interface IASTVisitor<T>
{
    T VisitBinOp(BinOp binOp);
    T VisitNum(Num num);
}

public abstract record AST
{
    public abstract T Accept<T>(IASTVisitor<T> visitor);
}