using LLVMApp.AST;
using LLVMSharp.Interop;

namespace LLVMApp.Contexts;

public unsafe record CodeGenVisitor : ExpressionVisitor
{
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

    public LLVMValueRef Pop() => _valueStack.Pop();

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

        //switch (node.NodeType)
        //{
        //    case ExpressionType.AdditionExpression:
        //        n = LLVM.BuildFAdd(_builder, l, r, "addtmp");
        //        break;
        //    case ExpressionType.SubtractExpression:
        //        n = LLVM.BuildFSub(_builder, l, r, "subtmp");
        //        break;
        //    case ExpressionType.MultiplyExpression:
        //        n = LLVM.BuildFMul(_builder, l, r, "multmp");
        //        break;
        //    case ExpressionType.LessThanExpression:
        //        // Convert bool 0/1 to double 0.0 or 1.0
        //        n = LLVM.BuildUIToFP(_builder, LLVM.BuildFCmp(_builder, LLVMRealPredicate.LLVMRealULT, l, r, "cmptmp"), LLVM.DoubleType(), "booltmp");
        //        break;
        //    default:
        //        throw new Exception("invalid binary operator");
        //}

        //_valueStack.Push(n);
        return node;
    }
}