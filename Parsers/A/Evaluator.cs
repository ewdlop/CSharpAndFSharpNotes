namespace Parsers.A;

public interface INodeVisitor
{
    int Visit(Node node);
}

public class Evaluator : INodeVisitor
{
    public int Visit(Node node) => node.Type switch
    {
        NodeType.Value => int.Parse(node.Value),
        NodeType.Operator => node.Value switch
        {
            "+" => node.Children[0].Accept(this) + node.Children[1].Accept(this),
            "-" => node.Children[0].Accept(this) - node.Children[1].Accept(this),
            "*" => node.Children[0].Accept(this) * node.Children[1].Accept(this),
            "/" => node.Children[0].Accept(this) / node.Children[1].Accept(this),
            _ => throw new Exception("Unknown operator")
        },
        _ => 0
    };
}