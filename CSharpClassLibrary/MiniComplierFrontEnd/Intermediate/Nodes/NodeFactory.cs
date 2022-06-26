using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokenizer;
using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

public class NodeFactory
{
    private readonly ITokenizer _lexerBehavior;

    public NodeFactory(ITokenizer lexerBehavior)
    {
        _lexerBehavior = lexerBehavior;
    }

    internal static Lazy<INode> Node { get; private set; } = new Lazy<INode>(() =>
    {
        Tokenizer dummyLexerBehavior = new();
        return new Node(dummyLexerBehavior);
    });

    public Node CreateNode()
    {
        return new Node(_lexerBehavior);
    }

    public static INode CreateNode(ITokenizer lexerBehavior)
    {
        return new Node(lexerBehavior);
    }

    public static INode GetDummyNode() => Node.Value;
}
