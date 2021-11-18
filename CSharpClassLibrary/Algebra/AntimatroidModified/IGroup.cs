namespace CSharpClassLibrary.Algebra.AntimatroidModified
{

    public interface IGroup<T>: IMonoid<T>
    {
        T Inverse(T t);
    }
}
