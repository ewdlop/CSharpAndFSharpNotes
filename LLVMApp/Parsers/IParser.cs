namespace LLVMApp.Parsers;

public interface IParser
{
    void HandleDefinition();
    void HandleExtern();
    void HandleTopLevelExpression();
}
