using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes
{
    public static class NodeFactory
    {
        internal static Lazy<Node> Node { get; private set; } = new();
        public static Node CreateNewNode()
        {
            Node = new();
            return Node.Value;
        }
        public static Node GetDummyNode() => Node.Value;
    }
}
