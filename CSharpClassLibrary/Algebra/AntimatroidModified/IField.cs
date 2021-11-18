namespace CSharpClassLibrary.Algebra.AntimatroidModified
{
    public interface IField<T, A, M> : IDivisionRing<T, A, M>
    where A : IAbelianGroup<T>
    where M : IAbelianGroup<T>
    {

    }
}
