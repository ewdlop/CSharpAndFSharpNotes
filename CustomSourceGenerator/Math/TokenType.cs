using System.Text;
using System.Text.RegularExpressions;
using Tokens = System.Collections.Generic.IEnumerable<CustomSourceGenerator.Math.Token>;
using SymbolTable = System.Collections.Generic.HashSet<string>;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CustomSourceGenerator.Math;

public enum TokenType
{
    Number,
    Identifier,
    Operation,
    OpenParens,
    CloseParens,
    Equal,
    EOL,
    EOF,
    Spaces,
    Comma,
    Sum,
    None
}

public struct Token
{
    public TokenType Type;
    public string Value;
    public int Line;
    public int Column;
}

/// <summary>
/// Not thread safe
/// </summary>
public static class Lexer
{

    public static void PrintTokens(Tokens tokens)
    {
        foreach (Token token in tokens)
        {
            Console.WriteLine($"{token.Line}, {token.Column}, {token.Type}, {token.Value}");
        }
    }

    private static readonly (TokenType Type, string Regex)[] TokenStrings = {
        (TokenType.EOL,         @"(\r\n|\r|\n)"),
        (TokenType.Spaces,      @"\s+"),
        (TokenType.Number,      @"[+-]?((\d+\.?\d*)|(\.\d+))"),
        (TokenType.Identifier,  @"[_a-zA-Z][`'""_a-zA-Z0-9]*"),
        (TokenType.Operation,   @"[\+\-/\*]"),
        (TokenType.OpenParens,  @"[([{]"),
        (TokenType.CloseParens, @"[)\]}]"),
        (TokenType.Equal,       @"="),
        (TokenType.Comma,       @","),
        (TokenType.Sum,         @"∑")
    };

    private static readonly IEnumerable<(TokenType Type, Regex Rule)> TokenExpressions =
        TokenStrings.Select(
            t => (t.Type, new Regex($"^{t.Regex}", RegexOptions.Compiled | RegexOptions.Singleline)));

    public static Tokens Tokenize(string source)
    {
        int currentLine = 1;
        int currentColumn = 1;
        while (source.Length > 0)
        {

            int matchLength = 0;
            TokenType tokenType = TokenType.None;
            string value = string.Empty;
            foreach ((TokenType type, Regex rule) in TokenExpressions)
            {
                Match match = rule.Match(source);
                if (match.Success)
                {
                    matchLength = match.Length;
                    tokenType = type;
                    value = match.Value;
                    break;
                }
            }

            if (matchLength == 0)
            {
                throw new Exception($"Unrecognized symbol '{source[currentLine - 1]}' at index {currentLine - 1} (line {currentLine}, column {currentColumn}).");
            }
            else
            {
                if (tokenType != TokenType.Spaces)
                {
                    yield return new Token
                    {
                        Type = tokenType,
                        Value = value,
                        Line = currentLine,
                        Column = currentColumn
                    };
                }
                currentColumn += matchLength;
                if (tokenType == TokenType.EOL)
                {
                    currentLine += 1;
                    currentColumn = 0;
                }
                source = source.Substring(matchLength);
            }
        }
        yield return new Token
        {
            Type = TokenType.EOF,
            Line = currentLine,
            Column = currentColumn
        };
    }
}
/// <summary>
/// Not thread safe
/// </summary>
public static class Parser
{
    public static string Parse(Tokens tokens)
    {
        SymbolTable globalSymbolTable = new SymbolTable();
        SymbolTable symbolTable = new SymbolTable();
        StringBuilder buffer = new StringBuilder();

        IEnumerator<Token> en = tokens.GetEnumerator();
        en.MoveNext();

        buffer = Lines(new Context
        {
            tokens = en,
            globalSymbolTable = globalSymbolTable,
            symbolTable = symbolTable,
            buffer = buffer
        });
        return buffer.ToString();

    }


    private readonly static string Preamble = @"
        using static System.Math;
        using static Maths.FormulaHelpers;
        namespace Maths {
            public static partial class Formulas { 
        ";

    private readonly static string Ending = @"}
    }";

    private struct Context
    {
        public IEnumerator<Token> tokens;
        public SymbolTable globalSymbolTable;
        public SymbolTable symbolTable;
        public StringBuilder buffer;
    }

    private static StringBuilder Error(Token token, TokenType type, string value = "") =>
        throw new Exception($"Expected {type} {(value == "" ? "" : $" with {token.Value}")} at {token.Line},{token.Column} Instead found {token.Type} with value {token.Value}");

    private static HashSet<string> ValidFunctions =
        new HashSet<string>(typeof(System.Math).GetMethods().Select(m => m.Name.ToLower()));

    private static Dictionary<string, string> ReplacementStrings = new Dictionary<string, string> {
        {"'''", "Third" }, {"''", "Second" }, {"'", "Prime"}
    };
    private static StringBuilder EmitIdentifier(Context context, Token token)
    {
        string tokenValue = token.Value;
        if (tokenValue == "pi")
        {
            context.buffer.Append("PI"); // Doesn't follow pattern
            return context.buffer;
        }
        if (ValidFunctions.Contains(tokenValue))
        {
            context.buffer.Append(char.ToUpper(tokenValue[0]) + tokenValue[1..]);
            return context.buffer;
        }

        string id = token.Value;
        if (context.globalSymbolTable.Contains(token.Value) ||
                      context.symbolTable.Contains(token.Value))
        {
            foreach ((string replacing, string replacement) in ReplacementStrings)
            {
                id = id.Replace(replacing, replacement);
            }
            return context.buffer.Append(id);
        }
        else
        {
            throw new Exception($"{token.Value} not a known identifier or function.");
        }
    }

    private static StringBuilder Emit(Context ctx, Token token) => token.Type switch
    {
        TokenType.EOL => ctx.buffer.Append('\n'),
        TokenType.CloseParens => ctx.buffer.Append(')'), // All parens become rounded
        TokenType.OpenParens => ctx.buffer.Append('('),
        TokenType.Equal => ctx.buffer.Append("=>"),
        TokenType.Comma => ctx.buffer.Append(token.Value),

        // Identifiers are normalized and checked for injection attacks
        TokenType.Identifier => EmitIdentifier(ctx, token),
        TokenType.Number => ctx.buffer.Append(token.Value),
        TokenType.Operation => ctx.buffer.Append(token.Value),
        TokenType.Sum => ctx.buffer.Append("MySum"),
        _ => Error(token, TokenType.None)
    };

    private static bool Peek(Context ctx, TokenType type, string value = "")
    {
        Token token = ctx.tokens.Current;

        return (token.Type == type && value == string.Empty) ||
           (token.Type == type && value == token.Value);
    }

    private static Token NextToken(Context ctx)
    {
        var token = ctx.tokens.Current;
        ctx.tokens.MoveNext();
        return token;
    }

    private static void Consume(Context ctx, TokenType type, string value = "")
    {

        var token = NextToken(ctx);

        if ((token.Type == type && value == string.Empty) ||
           (token.Type == type && value == token.Value))
        {

            ctx.buffer.Append(' ');
            Emit(ctx, token);
        }
        else
        {
            Error(token, type, value);
        }
    }

    private static StringBuilder Lines(Context ctx)
    {
        // lines    = {line} EOF

        ctx.buffer.Append(Preamble);

        while (!Peek(ctx, TokenType.EOF))
        {
            Line(ctx);
        }

        ctx.buffer.Append(Ending);

        return ctx.buffer;
    }

    private static void AddGlobalSymbol(Context ctx)
    {
        var token = ctx.tokens.Current;
        if (Peek(ctx, TokenType.Identifier))
        {
            ctx.globalSymbolTable.Add(token.Value);
        }
        else
        {
            Error(token, TokenType.Identifier);
        }
    }

    private static void AddSymbol(Context ctx)
    {
        var token = ctx.tokens.Current;
        if (Peek(ctx, TokenType.Identifier))
        {
            ctx.symbolTable.Add(token.Value);
        }
        else
        {
            Error(token, TokenType.Identifier);
        }
    }

    private static void Line(Context ctx)
    {
        // line    = {EOL} identifier [lround args rround] equal expr EOL {EOL}

        ctx.symbolTable.Clear();

        while (Peek(ctx, TokenType.EOL))
        {
            Consume(ctx, TokenType.EOL);
        }

        ctx.buffer.Append("\tpublic static double ");

        AddGlobalSymbol(ctx);
        Consume(ctx, TokenType.Identifier);

        if (Peek(ctx, TokenType.OpenParens, "("))
        {
            Consume(ctx, TokenType.OpenParens, "("); // Just round parens
            Args(ctx);
            Consume(ctx, TokenType.CloseParens, ")");
        }

        Consume(ctx, TokenType.Equal);
        Expression(ctx);
        ctx.buffer.Append(" ;");

        Consume(ctx, TokenType.EOL);

        while (Peek(ctx, TokenType.EOL))
        {
            Consume(ctx, TokenType.EOL);
        }
    }

    private static void Args(Context ctx)
    {
        // args    = identifier {comma identifier}
        // It doesn't make sense for a math function to have zero args (I think)

        ctx.buffer.Append("double ");
        AddSymbol(ctx);
        Consume(ctx, TokenType.Identifier);

        while (Peek(ctx, TokenType.Comma))
        {
            Consume(ctx, TokenType.Comma);
            ctx.buffer.Append("double ");
            AddSymbol(ctx);
            Consume(ctx, TokenType.Identifier);
        }
    }

    private readonly static Func<Context, string, bool> IsOperation = (ctx, op)
        => Peek(ctx, TokenType.Operation, op);
    private readonly static Action<Context, string> ConsOperation = (ctx, op)
        => Consume(ctx, TokenType.Operation, op);
    private static void Expression(Context ctx)
    {
        // expr    = [plus|minus] term { (plus|minus) term }

        if (IsOperation(ctx, "+")) ConsOperation(ctx, "+");
        if (IsOperation(ctx, "-")) ConsOperation(ctx, "-");

        Term(ctx);

        while (IsOperation(ctx, "+") || IsOperation(ctx, "-"))
        {

            if (IsOperation(ctx, "+")) ConsOperation(ctx, "+");
            if (IsOperation(ctx, "-")) ConsOperation(ctx, "-");

            Term(ctx);
        }
    }
    private static void Term(Context ctx)
    {
        // term    = factor { (times|divide) factor };
        Factor(ctx);

        while (IsOperation(ctx, "*") || IsOperation(ctx, "/"))
        {
            if (IsOperation(ctx, "*")) ConsOperation(ctx, "*");
            if (IsOperation(ctx, "/")) ConsOperation(ctx, "/");

            Term(ctx);
        }
    }

    private static void Factor(Context ctx)
    {
        // factor  = number | var | func | lround expr rround;
        if (Peek(ctx, TokenType.Number))
        {
            Consume(ctx, TokenType.Number);
            return;
        }

        if (Peek(ctx, TokenType.Identifier))
        {
            Consume(ctx, TokenType.Identifier); // Is either var or func
            if (Peek(ctx, TokenType.OpenParens, "("))
            { // Is Func, but we already consumed its name
                Funct(ctx);
            }
            return;
        }
        if (Peek(ctx, TokenType.Sum))
        {
            Sum(ctx);
            return;
        }
        // Must be a parenthesized expression
        Consume(ctx, TokenType.OpenParens);
        Expression(ctx);
        Consume(ctx, TokenType.CloseParens);
    }

    private static void Sum(Context ctx)
    {
        // sum     = ∑ lround identifier comma expr1 comma expr2 comma expr3 rround;
        // TODO: differentiate in the language between integer and double, but complicated for a sample.
        Consume(ctx, TokenType.Sum);
        Consume(ctx, TokenType.OpenParens, "(");

        AddSymbol(ctx);
        var varName = NextToken(ctx).Value;
        NextToken(ctx); // consume the first comma without emitting it

        ctx.buffer.Append("(int)");
        Expression(ctx); // Start index
        Consume(ctx, TokenType.Comma);

        ctx.buffer.Append("(int)");
        Expression(ctx); // End index
        Consume(ctx, TokenType.Comma);

        ctx.buffer.Append($"{varName} => "); // It needs to be a lambda

        Expression(ctx); // expr to evaluate at each iteration

        Consume(ctx, TokenType.CloseParens, ")");
    }
    private static void Funct(Context ctx)
    {
        // func    = identifier lround expr {comma expr} rround;
        Consume(ctx, TokenType.OpenParens, "(");
        Expression(ctx);
        while (Peek(ctx, TokenType.Comma))
        {
            Consume(ctx, TokenType.Comma);
            Expression(ctx);
        }
        Consume(ctx, TokenType.CloseParens, ")");
    }
}


