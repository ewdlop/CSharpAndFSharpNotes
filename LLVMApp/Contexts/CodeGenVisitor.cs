using LLVMApp.AST;
using LLVMSharp.Interop;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LLVMApp.Contexts;

public unsafe record CodeGenVisitor : ExpressionVisitor
{
    private const string addtmp = "addtmp";
    //private static readonly LLVMBool LLVMBoolFalse = new LLVMBool(0);
    private static readonly LLVMValueRef NullValue = new(IntPtr.Zero);
    private readonly LLVMModuleRef _module;
    private readonly LLVMBuilderRef _builder;
    private readonly Dictionary<string, LLVMValueRef> _namedValues = new();
    private readonly Stack<LLVMValueRef> _valueStack = new();
    public IReadOnlyCollection<LLVMValueRef> ResultStack => _valueStack;

    public CodeGenVisitor(LLVMModuleRef module, LLVMBuilderRef builder)
    {
        _module = module;
        _builder = builder;
    }

    public LLVMValueRef PopStack() => _valueStack.Pop();

    public void ClearResultStack() => _valueStack.Clear();

    public override ExpressionAST VisitNumberExpressionAST(NumberExpressionAST node)
    {

        ArgumentNullException.ThrowIfNull(node);
        _valueStack.Push(LLVM.ConstReal(LLVM.DoubleType(), node.Value));
        return node;
    }

    public override ExpressionAST VisitVariableExpressionAST(VariableExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        // Look this variable up in the function.
        if (_namedValues.TryGetValue(node.Name, out LLVMValueRef value))
        {
            _valueStack.Push(value);
        }
        else
        {
            throw new Exception("Unknown variable name");
        }

        return node;
    }

    public override ExpressionAST VisitBinaryExpressionAST(BinaryExpressionAST node)
    {
        ArgumentNullException.ThrowIfNull(node);
        Visit(node.Lhs);
        Visit(node.Rhs);

        LLVMValueRef r = _valueStack.Pop();
        LLVMValueRef l = _valueStack.Pop();

        LLVMValueRef n;

        IntPtr addtmpPtr;
        addtmpPtr = Marshal.StringToHGlobalAnsi(addtmp);
        //System.Security.SecureString s = new System.Security.SecureString();
        //Marshal.SecureStringToGlobalAllocUnicode(addtmpPtr);

        n = node.NodeType switch
        {
            ExpressionType.AdditionExpression => LLVM.BuildFAdd(_builder, l, r, (sbyte*)addtmpPtr),
            ExpressionType.SubtractExpression => LLVM.BuildFSub(_builder, l, r, (sbyte*)addtmpPtr),
            ExpressionType.MultiplyExpression => LLVM.BuildFMul(_builder, l, r, (sbyte*)addtmpPtr),
            ExpressionType.LessThanExpression => LLVM.BuildFCmp(_builder, LLVMRealPredicate.LLVMRealOLT, l, r, (sbyte*)addtmpPtr),
            _ => throw new ArgumentException($"operator {node.Operation} is not a valid operator")
        };
        _valueStack.Push(n);
        return node;
    }

    public override ExpressionAST VisitCallExpressionAST(CallExpressionAst node)
    {
        IntPtr nodeCallee;
        nodeCallee = Marshal.StringToHGlobalAnsi(addtmp);

        LLVMOpaqueValue* calleeF = LLVM.GetNamedFunction(_module, (sbyte*)nodeCallee);
        IntPtr calleeP = new IntPtr(calleeF);
        if (calleeP == IntPtr.Zero)
        {
            throw new Exception("Unknown function referenced");
        }

        ExpressionAST[] arguments = node.Arguments.ToArray();
        if (LLVM.CountParams(calleeF) != node.Arguments.Count())
        {
            throw new Exception("Incorrect # arguments passed");
        }

        int argumentCount = arguments.Length;
        LLVMOpaqueValue** argsV = stackalloc LLVMOpaqueValue*[Math.Max(argumentCount, 1)];
        for (int i = 0; i < argumentCount; ++i)
        {
            Visit(arguments[i]);
            argsV[i] = _valueStack.Pop();
        }

        IntPtr calltmp;
        calltmp = Marshal.StringToHGlobalAnsi("calltmp");
        LLVMOpaqueType* retType = LLVM.GetReturnType(LLVM.GetElementType(LLVM.TypeOf(calleeF)));
        _valueStack.Push(LLVM.BuildCall2(_builder, retType, calleeF, argsV, (uint)argumentCount, (sbyte*)calltmp));

        return node;
    }
}