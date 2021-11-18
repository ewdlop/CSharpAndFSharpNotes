namespace CSharpClassLibrary.Algebra.One
{
    public interface IField<T, A, M> : IDivisionRing<T, A, M>
    where A : IAbelianGroup<T>
    where M : IAbelianGroup<T>
    {

    }
}
