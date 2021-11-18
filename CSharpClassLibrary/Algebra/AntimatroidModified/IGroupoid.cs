namespace CSharpClassLibrary.Algebra.AntimatroidModified
{
    public interface IGroupoid<T>
    {
        T Operation(T a, T b);
    }
}
