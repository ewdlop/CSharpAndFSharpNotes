using System.Numerics;

namespace Parsers.C;

public abstract record ExpressionNode<T> where T: INumber<T>
{
    public abstract T Evaluate();
}
public record ConstantExpressionNode<T>(T Value) : ExpressionNode<T> where T: INumber<T>
{
    public override T Evaluate() => Value;
}

public record BinaryExpressionNode<T>(ExpressionNode<T> Left, ExpressionNode<T> Right, Func<T, T, T> Operation) : ExpressionNode<T>
     where T : INumber<T>
{
    public override T Evaluate() => Operation(Left.Evaluate(), Right.Evaluate());
}

public record AdditionExpressionNode<T> : BinaryExpressionNode<T> where T : INumber<T>
{
    public AdditionExpressionNode(ExpressionNode<T> left, ExpressionNode<T> right) : base(left, right, (a, b) => a + b){ }
}

public record SubtractionExpressionNode<T> : BinaryExpressionNode<T> where T : INumber<T>
{
    public SubtractionExpressionNode(ExpressionNode<T> left, ExpressionNode<T> right) : base(left, right, (a, b) => a - b) { }
}

public record MultiplicationExpressionNode<T> : BinaryExpressionNode<T> where T : INumber<T>
{
    public MultiplicationExpressionNode(ExpressionNode<T> left, ExpressionNode<T> right) : base(left, right, (a, b) => a * b) { }
}

public record DivisionExpressionNode<T> : BinaryExpressionNode<T> where T : INumber<T>
{
    public DivisionExpressionNode(ExpressionNode<T> left, ExpressionNode<T> right) : base(left, right, (a, b) => a / b) { }
}

public record BlockExpressionNode<T>(IEnumerable<ExpressionNode<T>> expressionNodes) : ExpressionNode<T> where T : INumber<T>
{
    public override T Evaluate()
    {
        T result = T.Zero;
        foreach (var expression in expressionNodes)
        {
            result = expression.Evaluate();
        }
        return result;
    }
}