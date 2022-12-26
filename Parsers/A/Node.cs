using System.Numerics;

namespace Parsers.A;

public abstract record Node<T>(NodeType Type, string Value, List<Node<T>> Children) where T: INumber<T>
{
    public Node(NodeType type, string value) : this(type, value, new List<Node<T>>()) {}
    public abstract T Accept(INodeVisitor<T> visitor);
}
public record ValueNode<T> : Node<T> where T : INumber<T>
{
    public ValueNode(string value) : base(NodeType.Value, value){ }
    public override T Accept(INodeVisitor<T> visitor) => visitor.Visit(this);
}

public record OperatorNode<T> : Node<T> where T : INumber<T>
{
    public OperatorNode(string value) : base(NodeType.Operator, value){}
    public override T Accept(INodeVisitor<T> visitor) => visitor.Visit(this);
}