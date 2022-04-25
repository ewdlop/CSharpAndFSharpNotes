using System.Reflection;

namespace LLVMApp.AST;

public record ASTContext(MethodInfo? MethodInfo, object? Instance, ExpressionAST? Arugmennt);