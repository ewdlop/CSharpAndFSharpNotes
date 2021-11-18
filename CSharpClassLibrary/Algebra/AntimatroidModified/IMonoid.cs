namespace CSharpClassLibrary.Algebra.AntimatroidModified
{

    public interface IMonoid<T>: ISemigroup<T>
    {
        T Identity { get; }
    }
}
