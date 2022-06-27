namespace LLVMApp.Parsers;

using LLVMApp.AST;
using System;
using System.Collections.Generic;

public class BaseParserListener
{
    private static readonly Type IParserListenerType = typeof(IParserListener);
    private readonly Stack<string> _descentStack = new();
    private readonly Stack<ASTContext> _ascentStack = new();
    private readonly IParserListener _parserListener;
    public BaseParserListener(IParserListener parserListener)
    {
        _parserListener = parserListener;
    }

    public void EnterRule(string ruleName) => _descentStack.Push(ruleName);
    public void ExitRule(ExpressionAST argument)
    {
        string ruleName = _descentStack.Pop();
        _ascentStack.Push(new ASTContext(IParserListenerType.GetMethod("Exit" + ruleName), _parserListener, argument));
        _ascentStack.Push(new ASTContext(IParserListenerType.GetMethod("Enter" + ruleName), _parserListener, argument));
    }
    public void Listen()
    {
        if (_parserListener is not null)
        {
            while (_ascentStack.Count > 0)
            {
                ASTContext context = _ascentStack.Pop();
                if (context.Arugmennt is not null)
                {
                    context.MethodInfo?.Invoke(context.Instance, new object[] { context.Arugmennt });
                }
            }
        }
    }
}