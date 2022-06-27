using System;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokenizer;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

public class Emitter : ILabelEmitter
{
    private readonly ITokenizer _tokenzier;
    public int Lexline { get; init; }
    public Emitter(ITokenizer tokenzier)
    {
        _tokenzier = tokenzier;
        Lexline = _tokenzier.Line;
    }
    private int Labels { get; set; }
    public void Error(string error) => throw new Exception($"near line {Lexline}: {error}");
    public int NewLabel() => ++Labels;
    public virtual void EmitLabel(int i) => Console.Write($"L{i}:");
    public virtual void Emit(string s) => Console.WriteLine($"\t{s}");
}
