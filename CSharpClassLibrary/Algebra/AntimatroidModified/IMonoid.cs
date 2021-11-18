namespace CSharpClassLibrary.Algebra.One
{

    public interface IMonoid<T>: ISemigroup<T>
    {
        T Identity { get; }
    }
}
