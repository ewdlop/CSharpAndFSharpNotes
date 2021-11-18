namespace CSharpClassLibrary.Algebra.One
{
    public interface IDivisionRing<T, A, M> : IRingWithUnity<T, A, M>
    where A : IGroup<T>
    where M : IGroup<T>
    {

    }
}
