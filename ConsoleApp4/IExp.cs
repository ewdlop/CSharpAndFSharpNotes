using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Expression
{ 
    public interface IExpression<T> where T : INumber<T>
    {
        T Eval();
    }

    public record LiteralExpression<T> : IExpression<T> where T : INumber<T>
    {
        public LiteralExpression(T n)
        {
            N = n;
        }

        public T N { get; set; }

        public T Eval() => N;
    }

    public record AddExpression<T> : IExpression<T> where T : INumber<T>
    {
        public AddExpression(IExpression<T> a, IExpression<T> b)
        {
            A = a;
            B = b;
        }

        public IExpression<T> A { get; }
        public IExpression<T> B { get; }

        public T Eval() => A.Eval() + B.Eval();
    }

    public record MultiplyExpression<T> : IExpression<T> where T : INumber<T>
    {
        public MultiplyExpression(IExpression<T> a, IExpression<T> b)
        {
            A = a;
            B = b;
        }

        public IExpression<T> A { get; }
        public IExpression<T> B { get; }

        public T Eval() => A.Eval() * B.Eval();
    }
}


namespace Expression2
{

    public interface IExpression<T> where T : INumber<T>
    {
        
    }
    public interface IUnaryExpression<T> : IExpression<T> where T : INumber<T>
    {
        public static abstract T Eval(T a);
    }
    public interface IBinaryExpression<T> : IExpression<T> where T : INumber<T>
    {
        public static abstract T Eval(T a, T b);
    }

    public record LiteralExpression<T> : IExpression<T> where T : INumber<T>
    {
        public static T Eval(T a) => a;

    }

    public record AddExpression<T> : IBinaryExpression<T> where T : INumber<T>
    {

        public static T Eval(T a, T b) => a + b;
    }

    public record MultiplyExpression<T> : IBinaryExpression<T> where T : INumber<T>
    {

        public static T Eval(T a, T b) => a * b;
    }
}

namespace Expression3
{
    public interface IExprAlgebra<T1,T2> where T2 : INumber<T2>
    {
        static abstract T1 Literal(T2 n);
        static abstract T1 Add(T1 a, T1 b);
    }

    public interface IEvalExpression<T>
    {
        T Eval();
    }

    public class Expression<T> : IEvalExpression<T> where T : INumber<T>
    {
        public Expression(Func<T> eval)
        {
            _eval = eval;
        }
        Func<T> _eval;

        public T Eval()
            => _eval();
    }
    public interface IPrintExpression
    {
       public string? Print();
    }

    public class PrintExpression<T> : IPrintExpression 
    {
        public PrintExpression(Func<string?> print)
        {
            _print = print;
        }
        Func<string?> _print;

        public string? Print()
            => _print();
    }
    
    public class EvalAlgebra<T> : IExprAlgebra<IEvalExpression<T>,T> where T : INumber<T>
    {
        public static IEvalExpression<T> Literal(T n)
            => new Expression<T>(() => n);

        public static IEvalExpression<T> Add(IEvalExpression<T> a, IEvalExpression<T> b)
            => new Expression<T>(() => a.Eval() + b.Eval());
    }

    public class PrintAlgebra<T> : IExprAlgebra<PrintExpression<T>,T> where T : INumber<T>
    {
        public static PrintExpression<T> Literal(T n)
            => new PrintExpression<T>(n.ToString);

        public static PrintExpression<T> Add(PrintExpression<T> a, PrintExpression<T> b)
            => new PrintExpression<T>(() => $"({a.Print()} + {b.Print()})");
    }

    public interface IExprAlgebraExt<T1,T2> : IExprAlgebra<T1, T2> where T2 : INumber<T2>
    {
        abstract static T1 Mult(T1 a, T1 b);
    }

    public class EvalAlgebraExt<T> : EvalAlgebra<T>, IExprAlgebraExt<IEvalExpression<T>,T> where T : INumber<T>
    {
        public static IEvalExpression<T> Mult(IEvalExpression<T> a, IEvalExpression<T> b)
            => new Expression<T>(() => a.Eval() * b.Eval());
    }
}

namespace Expression4
{
    public interface IExprAlgebra<T1, T2> where T2 : INumber<T2>
    {
        static abstract T1 Literal(T2 n);
        static abstract T1 Add(T1 a, T1 b);
    }

    public interface IEvalExpression<T>
    {
        T Eval();
    }

    public record Expression<T> : IEvalExpression<T> where T : INumber<T>
    {
        public Expression(Func<T> eval)
        {
            LazyValue = new Lazy<T>(() => eval());
        }
        private Lazy<T> LazyValue { get; init; }

        public T Eval() => LazyValue.Value;
    }
    public interface IPrintExpression
    {
        public string? Print();
    }

    public class PrintExpression<T> : IPrintExpression
    {
        public PrintExpression(Func<string?> print)
        {
            LazyValue = new Lazy<string?>(() => print());
        }
        private Lazy<string?> LazyValue { get; init; }

        public string? Print() => LazyValue.Value;
    }

    public class EvalAlgebra<T> : IExprAlgebra<IEvalExpression<T>, T> where T : INumber<T>
    {
        public static IEvalExpression<T> Literal(T n)
            => new Expression<T>(() => n);

        public static IEvalExpression<T> Add(IEvalExpression<T> a, IEvalExpression<T> b)
            => new Expression<T>(() => a.Eval() + b.Eval());
    }

    public class PrintAlgebra<T> : IExprAlgebra<PrintExpression<T>, T> where T : INumber<T>
    {
        public static PrintExpression<T> Literal(T n)
            => new PrintExpression<T>(n.ToString);

        public static PrintExpression<T> Add(PrintExpression<T> a, PrintExpression<T> b)
            => new PrintExpression<T>(() => $"({a.Print()} + {b.Print()})");
    }

    public interface IExprAlgebraExt<T1, T2> : IExprAlgebra<T1, T2> where T2 : INumber<T2>
    {
        abstract static T1 Mult(T1 a, T1 b);
    }

    public class EvalAlgebraExt<T> : EvalAlgebra<T>, IExprAlgebraExt<IEvalExpression<T>, T> where T : INumber<T>
    {
        public static IEvalExpression<T> Mult(IEvalExpression<T> a, IEvalExpression<T> b)
            => new Expression<T>(() => a.Eval() * b.Eval());
    }
}

namespace Expression5 //memozie and static?
{
    public interface IExprAlgebra<T1, T2> where T2 : INumber<T2>
    {
        static abstract T1 Literal(T2 n);
        static abstract T1 Add(T1 a, T1 b);
    }

    public interface IEvalExpression<T>
    {
        T Eval();
    }

    public record Expression<T> : IEvalExpression<T> where T : INumber<T>
    {
        public Expression(Func<T> eval)
        {
            LazyValue = new Lazy<T>(() => eval());
        }
        private Lazy<T> LazyValue { get; init; }

        public T Eval() => LazyValue.Value;
    }
    public interface IPrintExpression
    {
        public string? Print();
    }

    public class PrintExpression<T> : IPrintExpression
    {
        public PrintExpression(Func<string?> print)
        {
            LazyValue = new Lazy<string?>(() => print());
        }
        private Lazy<string?> LazyValue { get; init; }

        public string? Print() => LazyValue.Value;
    }

    public class EvalAlgebra<T> : IExprAlgebra<IEvalExpression<T>, T> where T : INumber<T>
    {
        public static IEvalExpression<T> Literal(T n)
            => new Expression<T>(() => n);

        public static IEvalExpression<T> Add(IEvalExpression<T> a, IEvalExpression<T> b)
            => new Expression<T>(() => a.Eval() + b.Eval());
    }

    public class PrintAlgebra<T> : IExprAlgebra<PrintExpression<T>, T> where T : INumber<T>
    {
        public static PrintExpression<T> Literal(T n)
            => new PrintExpression<T>(n.ToString);

        public static PrintExpression<T> Add(PrintExpression<T> a, PrintExpression<T> b)
            => new PrintExpression<T>(() => $"({a.Print()} + {b.Print()})");
    }

    public interface IExprAlgebraExt<T1, T2> : IExprAlgebra<T1, T2> where T2 : INumber<T2>
    {
        abstract static T1 Mult(T1 a, T1 b);
    }

    public class EvalAlgebraExt<T> : EvalAlgebra<T>, IExprAlgebraExt<IEvalExpression<T>, T> where T : INumber<T>
    {
        public static IEvalExpression<T> Mult(IEvalExpression<T> a, IEvalExpression<T> b)
            => new Expression<T>(() => a.Eval() * b.Eval());
    }
}