using System;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface INodeEmitable
    {
        void EmitLabel(int i);
        void Emit(string s);
    }
    public interface INode : INodeEmitable
    {
        int Lexline { get; }
        void Error(string error);
        int NewLabel();
    }
    public abstract record Node : INode
    {
        private readonly ILexerCharacterReader _lexerCharacterReader;
        public int Lexline { get; }
        public Node()
        {
            _lexerCharacterReader = new LexerCharacterReader();
            Lexline = _lexerCharacterReader.Line;
        }
        private static int Labels { get; set; }
        void INode.Error(string error) => throw new Exception($"near line {Lexline}: {error}");
        public int NewLabel() => ++Labels;
        public void EmitLabel(int i) => Console.Write($"L{i}:");
        public void Emit(string s) => Console.WriteLine($"\t{s}");
    }
}
