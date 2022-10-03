// See https://aka.ms/new-console-template for more information

Not<Not<int>> proof_of_not_not_int = CreateDoubleNegative(5);

Absurd absurd = proof_of_not_not_int.Apply(new Not<int>(triple_cheat));

for (var n = 0; n <= 10; n++)
{
    FibonacciCps(n, fib => Console.WriteLine($"{n}: {fib}"));
}

B ModusPonens<A, B>(Func<A, B> A_implies_B, A proof_of_A)
{
    return A_implies_B(proof_of_A);
}

Func<A, C> Syllogism<A, B, C>(
    Func<A, B> A_implies_B,
    Func<B, C> B_implies_C)
{
    return A => B_implies_C(A_implies_B(A));
}

Not<A> ModusTollens<A, B>(
    Func<A, B> A_implies_B,
    Not<B> not_B)
{
    return new Not<A>(a => not_B.Apply(A_implies_B(a)));
}

Absurd triple_cheat(int n)
{
    var product = n * 3;
    Console.WriteLine($"Product is {product}");
    throw new Exception("Can't ever return");
}

//Either<A, Not<A>> ExcludedMiddle<A>()
//{
//    // implementation?
//    return new Either<A, Not<A>>(new Not<A>(a => new Absurd()));
//}
Not<Not<A>> CreateDoubleNegative<A>(A proof_of_A)
{
    return new Not<Not<A>>(not_not_A => not_not_A.Apply(proof_of_A));
}
void FibonacciCps(int n, Action<int> continuation)
{
    switch (n)
    {
        case 0: continuation(0); break;
        case 1: continuation(1); break;
        default:
            FibonacciCps(n - 1, fib1 =>
                FibonacciCps(n - 2, fib2 =>
                    continuation(fib1 + fib2)));
            break;
    }
}
//void FibonacciCps(int n, Action<int> continuation) => n switch
//{
//    0 => continuation(0),
//    1 => continuation(1),
//    _ => FibonacciCps(n - 1, n1 => FibonacciCps(n - 2, n2 => continuation(n1 + n2)))
//};
class Not<A>
{
    public Not(Func<A, Absurd> A_implies_absurd)
    {
        _func = A_implies_absurd;
    }

    public Absurd Apply(A proof_of_A)
        => _func(proof_of_A);

    private readonly Func<A, Absurd> _func;
}

public class Either<TL, TR>
{
    private readonly TL _left;
    private readonly TR _right;
    private readonly bool _isLeft;

    public Either(TL left)
    {
        _left = left;
        _isLeft = true;
    }

    public Either(TR right)
    {
        _right = right;
        _isLeft = false;
    }
}
enum Absurd
{
}