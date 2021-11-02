namespace CSharpClassLibrary.Algebra.One
{

    public interface IGroup<T>: IMonoid<T>
    {
        T Inverse(T t);
    }
}
