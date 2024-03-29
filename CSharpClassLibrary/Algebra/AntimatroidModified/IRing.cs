﻿namespace CSharpClassLibrary.Algebra.AntimatroidModified
{
    public interface IRing<T, A, M> : ISemiring<T, A, M>
        where A : IGroup<T>
        where M : IMonoid<T>
    {

    }
}
