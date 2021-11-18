namespace CSharpClassLibrary.Algebra.One.ZnZ
{
    public class MultiplicativeIntegerMonoid : MultiplicativeIntegerSemigroup, IMonoid<long>
    {
        public long Identity => 1L;
    }
}
