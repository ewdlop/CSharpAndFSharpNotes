using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Expressions
{
    public record AccessingOperationExpression(IdExpression ArrayExpression, Expression IndexExpression, TypeToken TypeToken)
        : OperationExpression(new WordToken("[]", TokenTag.INDEX),TypeToken)
    {
        public override Expression Generate() => new AccessingOperationExpression(ArrayExpression, IndexExpression.Reduce(), TypeToken);
        public override void Jumping(int t, int f) => EmitJumps(Reduce().ToString(), t, f);
        public override string ToString() => $"{ArrayExpression} [ {IndexExpression} ]";
    }
}
