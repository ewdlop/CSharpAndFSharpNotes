﻿namespace CSharpClassLibrary.Algebra.One
{

    public interface IRingoid<T, A, M>
        where A : IGroupoid<T>
        where M : IGroupoid<T>
    {
        A Addition { get; }
        M Multiplication { get; }
        T Distribute(T a, T b);
    }
}
