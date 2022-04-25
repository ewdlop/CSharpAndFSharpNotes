namespace LLVMApp.Lexers;

public interface ILexer
{
    int? CurrentToken { get; }

    string? LastIdentifier { get; }

    double? LastNumberValue { get; }

    int? GetTokenPrecedence();

    int? GetNextToken();
}
