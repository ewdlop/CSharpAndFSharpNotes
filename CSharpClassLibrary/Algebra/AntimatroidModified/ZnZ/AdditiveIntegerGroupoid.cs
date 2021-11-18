namespace CSharpClassLibrary.Algebra.One.ZnZ
{
    public class AdditiveIntegerGroupoid : IGroupoid<long>
    {
        public long Operation(long a, long b) => a + b;
    }

}
