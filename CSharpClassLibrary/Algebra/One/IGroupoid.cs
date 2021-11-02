namespace CSharpClassLibrary.Algebra.One
{
    public interface IGroupoid<T>
    {
        T Operation(T a, T b);
    }
}
