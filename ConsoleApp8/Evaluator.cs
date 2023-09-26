namespace ConsoleApp8;

public class Evaluator : IASTVisitor<double>
{
    public double VisitBinOp(BinOp binOp)
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

    public double VisitNum(Num num)
    {
        return double.Parse(num.Value.Value ?? string.Empty);
    }
}