using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behaviors;
using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

public class NodeFactory
{
    private readonly ILexerBehavior _lexerBehavior;

    public NodeFactory(ILexerBehavior lexerBehavior)
    {
        _lexerBehavior = lexerBehavior;
    }

    internal static Lazy<INode> Node { get; private set; } = new Lazy<INode>(() =>
    {
        LexerBehavior dummyLexerBehavior = new();
        return new Node(dummyLexerBehavior);
    });

    public Node CreateNode()
    {
        return new Node(_lexerBehavior);
    }

    public static INode CreateNode(ILexerBehavior lexerBehavior)
    {
        return new Node(lexerBehavior);
    }

    public static INode GetDummyNode() => Node.Value;
}
