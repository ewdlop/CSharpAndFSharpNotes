using LLVMApp.AST;
using LLVMApp.Parsers;
using LLVMSharp.Interop;

namespace LLVMApp.Contexts;

public unsafe class CodeGenParserListener : IParserListener
{
    private readonly CodeGenVisitor _visitor;

    private readonly LLVMExecutionEngineRef _ee;

    private readonly LLVMPassManagerRef _passManager;

    public CodeGenParserListener(
        LLVMExecutionEngineRef ee, 
        LLVMPassManagerRef passManager,
        CodeGenVisitor visitor)
    {
        _visitor = visitor;
        _ee = ee;
        _passManager = passManager;
    }

    public void EnterHandleDefinition(FunctionExpressionAST data)
    {
        _visitor.Visit(data);
        LLVMValueRef function = _visitor.ResultStack.Pop();
        LLVM.DumpValue(function);

        LLVM.RunFunctionPassManager(_passManager, function);
        LLVM.DumpValue(function); // Dump the function for exposition purposes.
    }

    public void EnterHandleExtern(PrototypeExpressionAST data)
    {
        throw new NotImplementedException();
    }

    public void EnterHandleTopLevelExpression(FunctionExpressionAST data)
    {
        throw new NotImplementedException();
    }

    public void ExitHandleDefinition(FunctionExpressionAST data)
    {
        throw new NotImplementedException();
    }

    public void ExitHandleExtern(PrototypeExpressionAST data)
    {
        throw new NotImplementedException();
    }

    public void ExitHandleTopLevelExpression(FunctionExpressionAST data)
    {
        throw new NotImplementedException();
    }
}

public record CodeGenVisitor : ExpressionVisitor
{
    //private static readonly LLVMBool LLVMBoolFalse = new LLVMBool(0);
    private static readonly LLVMValueRef NullValue = new LLVMValueRef(IntPtr.Zero);
    private readonly LLVMModuleRef _module;
    private readonly LLVMBuilderRef _builder;
    private readonly Dictionary<string, LLVMValueRef> _namedValues = new();
    private readonly Stack<LLVMValueRef> _valueStack = new();
    public Stack<LLVMValueRef> ResultStack => _valueStack;

    public CodeGenVisitor(LLVMModuleRef module, LLVMBuilderRef builder)
    {
        _module = module;
        _builder = builder;
    }

    public void ClearResultStack() => _valueStack.Clear();

    public override ExpressionAST? VisitNumberExpressionAST(NumberExpressionAST? node)
    {
        //_valueStack.Push(LLVM.ConstReal(LLVM.DoubleType(), node.Value.));
        return node;
    }

    public override ExpressionAST? VisitVariableExpressionAST(VariableExpressionAST? node)
    {
        LLVMValueRef value;
        if (node is null || node.Name is null) return null;
        // Look this variable up in the function.
        if (_namedValues.TryGetValue(node?.Name, out value))
        {
            _valueStack.Push(value);
        }
        else
        {
            throw new Exception("Unknown variable name");
        }

        return node;
    }

    public override ExpressionAST? VisitBinaryExpressionAST(BinaryExpressionAST? node)
    {
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