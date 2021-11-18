namespace CSharpClassLibrary.Algebra.One.ZnZ
{
    public class AdditiveIntegerMonoid : AdditiveIntegerSemigroup, IMonoid<long>
    {
        public long Identity => 0L;
    }

}
