using System;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Behavior;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes
{
    public class Node : INode
    {
        private readonly ILexerBehavior _lexerBehavior;
        public int Lexline { get; }
        public Node()
        {
            //_lexerBehavior = lexerBehavior;'
            //need to fix this
            _lexerBehavior = new LexerBehavior();
            Lexline = _lexerBehavior.Line;
        }
        private static int Labels { get; set; }
        public void Error(string error) => throw new Exception($"near line {Lexline}: {error}");
        public int NewLabel() => ++Labels;
        public virtual void EmitLabel(int i) => Console.Write($"L{i}:");
        public virtual void Emit(string s) => Console.WriteLine($"\t{s}");
    }
}
