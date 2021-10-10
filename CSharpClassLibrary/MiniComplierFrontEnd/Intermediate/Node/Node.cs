using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface INodeEmitable
    {
        public void EmitLable(int i);
        public void Emit(string s);
    }
    public interface INode : INodeEmitable
    {
        int Lexline { get; init; }
        void Error(string error);
        public int NewLabel();
    }
    public record Node : INode
    {
        int INode.Lexline { get; init; } = Lexer.Lexer.LINE;
        static int Labels { get; set; }
        void INode.Error(string error)
        {
            throw new Exception($"near line {(this as INode).Lexline}: {error}");
        }
        public int NewLabel()
        {
            return ++Labels;
        }
        public void EmitLable(int i) => Console.Write($"L{i}:");
        public void Emit(string s) => Console.WriteLine($"\t{s}");
    }
}
