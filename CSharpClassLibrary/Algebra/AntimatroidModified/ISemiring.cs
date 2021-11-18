namespace CSharpClassLibrary.Algebra.AntimatroidModified
{
    public interface ISemiring<T, A, M> : IRingoid<T, A, M>
        where A : IMonoid<T>
        where M : IMonoid<T>
    {

    }
}
