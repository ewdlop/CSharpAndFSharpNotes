using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behavior;
using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes
{
    public class NodeFactory
    {
        private readonly LexerBehavior _lexerBehavior;

        public NodeFactory(LexerBehavior lexerBehavior)
        {
            _lexerBehavior = lexerBehavior;
        }

        internal static Lazy<INode> Node { get; private set; } = new Lazy<INode>(() =>
        {
            LexerBehavior dummyLexerBehavior = new();
            return new Node(dummyLexerBehavior);
        });

        public INode CreateNode()
        {
            return new Node(_lexerBehavior);
        }

        public static INode CreateNode(LexerBehavior lexerBehavior)
        {
            return new Node(lexerBehavior);
        }

        public static INode GetDummyNode() => Node.Value;
    }
}
