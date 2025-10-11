#if false
for (var n = 0; n <= 10; ++n)
{
    FibonacciCps(n, fib => Console.WriteLine($"{n}: {fib}"));
}

static void FibonacciCps(int n, Action<int> continuation)
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

#endif