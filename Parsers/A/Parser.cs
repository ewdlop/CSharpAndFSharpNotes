using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Parsers.A;

public class Parser
{
    public static Node<T>? Parse<T>(Stack<Token> tokenStack) where T:INumber<T>
    {
        var tokens = Reverse(tokenStack);
        Node<T>? previous = null;
        while (tokens.Count > 0)
        {
            //// Get the current token
            Token current = tokens.Pop();
            Node<T>? currentNode = null;
            if (current.Type == TokenType.Number)
            {
                currentNode = new ValueNode<T>(current.Value);
                if (previous is not null && previous.Type == NodeType.Operator)
                {
                    previous.Children.Add(currentNode);
                }
                else
                {
                    previous = currentNode;
                }
            }
            else if (current.Type == TokenType.Symbol)
            {
                currentNode = new OperatorNode<T>(current.Value);
                if(previous is not null)
                {
                    currentNode.Children.Add(previous);
                }
                previous = currentNode;
            }
        }
        return previous;
    }

    public static Stack<T> Reverse<T>(Stack<T> stack)
    {
        Stack<T> newStack = new Stack<T>();
        while (stack.Count > 0)
        {
            newStack.Push(stack.Pop());
        }
        return newStack;
    }
}