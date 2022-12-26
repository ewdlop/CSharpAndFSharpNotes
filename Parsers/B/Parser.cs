using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;

namespace Parsers.B
{
    public class Parser
    {
        public static LambdaExpression Parse<T>(string expression) where T : INumber<T>
        {
            ParameterExpression x = Expression.Parameter(typeof(INumber<T>), "x");
            Stack<Expression> stack = new Stack<Expression>();
            foreach(char c in expression)
            {
                if (char.IsDigit(c))
                {
                    stack.Push(Expression.Constant(T.Parse(c.ToString(),CultureInfo.InvariantCulture)));
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    Expression right = stack.Pop();
                    Expression left = stack.Pop();
                    stack.Push(c switch
                    {
                        '+' => Expression.Add(left, right),
                        '-' => Expression.Subtract(left, right),
                        '*' => Expression.Multiply(left, right),
                        '/' => Expression.Divide(left, right),
                        _ => throw new InvalidOperationException(),
                    });
                }
                else if (c == '(')
                {
                    stack.Push(Expression.Block(new List<ParameterExpression>()));
                }
                else if (c == ')')
                {
                    BlockExpression? block = stack.Pop() as BlockExpression;
                    if (block is null)
                    {
                        throw new InvalidOperationException();
                    }
                    block = Expression.Block(block.Variables, block.Expressions.Concat(new[] { stack.Pop() }));
                    stack.Push(block);
                }
            }
            // Return the top expression on the stack as a lambda expression
            return Expression.Lambda(stack.Pop(), x);
        }
    }
}
