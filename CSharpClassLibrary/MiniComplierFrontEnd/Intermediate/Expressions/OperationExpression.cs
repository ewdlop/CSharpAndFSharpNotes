using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record OperationExpression(Token Token, TypeToken TypeToken, ILabelEmitter Node) : Expression(Token,TypeToken, Node)
{
    public override Expression Reduce()
    {
        var expression = base.Generate();
        var tempExpressionNode = new TemporaryExpression(TypeToken, Node);
        Node.Emit($"{tempExpressionNode} = {expression}");
        return tempExpressionNode;
    }
    public override string ToString() => base.ToString();
}
