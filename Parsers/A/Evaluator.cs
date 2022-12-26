using System.Globalization;
using System.Numerics;

namespace Parsers.A;

public interface INodeVisitor<T> where T: INumber<T>
{
    T Visit(Node<T> node);
}

public class Evaluator<T> : INodeVisitor<T> where T : INumber<T>
{
    public T Visit(Node<T> node) => node.Type switch
    {
        NodeType.Value => T.Parse(node.Value, CultureInfo.CurrentCulture),
        NodeType.Operator => node.Value switch
        {
            "+" => node.Children[0].Accept(this) + node.Children[1].Accept(this),
            "-" => node.Children[0].Accept(this) - node.Children[1].Accept(this),
            "*" => node.Children[0].Accept(this) * node.Children[1].Accept(this),
            "/" => node.Children[0].Accept(this) / node.Children[1].Accept(this),
            _ => throw new Exception("Unknown operator")
        },
        _ => T.Zero
    };
}