using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions
{
    public record AccessingOperationExpression(IdExpression ArrayExpression, IExpression IndexExpression, TypeToken TypeToken, INode Node)
        : OperationExpression(new WordToken("[]", TokenTag.INDEX),TypeToken, Node)
    {
        public override Expression Generate() => new AccessingOperationExpression(ArrayExpression, IndexExpression.Reduce(), TypeToken, Node);
        public override void Jumping(int t, int f) => EmitJumps(Reduce().ToString(), t, f);
        public override string ToString() => $"{ArrayExpression} [ {IndexExpression} ]";
    }
}
