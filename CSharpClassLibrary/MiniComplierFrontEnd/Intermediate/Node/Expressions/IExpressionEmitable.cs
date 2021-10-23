using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{ 
    public interface IExpressionEmitable
    {
        void EmitJumps(string test, int t, int f);
    }
}
