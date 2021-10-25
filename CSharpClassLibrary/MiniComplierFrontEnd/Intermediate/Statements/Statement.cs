﻿using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements
{
    public class Statement: IStatement
    {
        internal static readonly IStatement NullStatement = new Statement(NodeFactory.GetDummyNode());
        internal static IStatement EnclosingStatement = NullStatement;
        protected readonly Node _node;
        public Statement(Node node)
        {
            _node = node;
        }
        public int After { get; protected set; }
        public virtual void Generate(int b, int a) { }
        public virtual void Init(IExpression expression, IStatement statement) { }
    }
}
