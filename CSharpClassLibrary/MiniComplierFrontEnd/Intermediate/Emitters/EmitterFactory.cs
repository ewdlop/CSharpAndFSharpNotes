using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokenizer;
using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

public class EmitterFactory
{
    private readonly ITokenizer _tokenizer;

    public EmitterFactory(ITokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    internal static Lazy<ILabelEmitter> Emitter { get; private set; } = new Lazy<ILabelEmitter>(() =>
    {
        Tokenizer dummyTokenzier = new();
        return new Emitter(dummyTokenzier);
    });

    public Emitter CreateEmitter()
    {
        return new Emitter(_tokenizer);
    }

    public static ILabelEmitter CreateEmitter(ITokenizer tokenizer)
    {
        return new Emitter(tokenizer);
    }

    public static ILabelEmitter GetDummyEmitter() => Emitter.Value;
}
