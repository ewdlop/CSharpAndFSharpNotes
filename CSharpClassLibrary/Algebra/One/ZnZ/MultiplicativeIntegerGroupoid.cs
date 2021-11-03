namespace CSharpClassLibrary.Algebra.One.ZnZ
{
    public class MultiplicativeIntegerGroupoid : IGroupoid<long>
    {
        public long Operation(long a, long b) => a * b;
    }
}
