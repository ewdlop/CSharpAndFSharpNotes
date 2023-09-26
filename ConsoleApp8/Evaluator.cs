using System.Globalization;
using System.Numerics;

namespace ConsoleApp8;

public class Evaluator<T> : IASTVisitor<T> where T : INumber<T>
{
    public T VisitBinOp(BinOp<T> binOp)
    {
        if (binOp.Op.Type == TokenType.Plus)
            return binOp.Left.Accept(this) + binOp.Right.Accept(this);
        if (binOp.Op.Type == TokenType.Minus)
            return binOp.Left.Accept(this) - binOp.Right.Accept(this);
        if (binOp.Op.Type == TokenType.Times)
            return binOp.Left.Accept(this) * binOp.Right.Accept(this);
        if (binOp.Op.Type == TokenType.Div)
            return binOp.Left.Accept(this) / binOp.Right.Accept(this);

        throw new Exception($"Unknown operator: {binOp.Op.Type}");
    }

    public T VisitNum(Num<T> num)
    {
        return T.TryParse(num.Value.Value ?? string.Empty, CultureInfo.CurrentCulture.NumberFormat,out T? result) ? result : T.Zero;
    }
}