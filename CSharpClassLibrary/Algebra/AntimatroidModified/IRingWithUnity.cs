namespace CSharpClassLibrary.Algebra.AntimatroidModified
{
    public interface IRingWithUnity<T, A, M> : IRing<T, A, M>
        where A : IGroup<T>
        where M : IMonoid<T>
    {

    }
}
