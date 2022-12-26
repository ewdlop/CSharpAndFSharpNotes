namespace Parsers.A;

public abstract record Node(NodeType Type, string Value, List<Node> Children)
{
    public Node(NodeType type, string value) : this(type, value, new List<Node>()) {}
    public abstract int Accept(INodeVisitor visitor);
}
public record ValueNode : Node
{
    public ValueNode(string value) : base(NodeType.Value, value){ }
    public override int Accept(INodeVisitor visitor) => visitor.Visit(this);
}

public record OperatorNode : Node
{
    public OperatorNode(string value) : base(NodeType.Operator, value){}
    public override int Accept(INodeVisitor visitor) => visitor.Visit(this);
}