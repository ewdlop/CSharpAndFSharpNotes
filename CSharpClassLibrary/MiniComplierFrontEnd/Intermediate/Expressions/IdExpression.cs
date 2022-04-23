using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record IdExpression(Token Token, TypeToken TypeToken, int Offset, INode Node) : Expression(Token, TypeToken, Node) { }
