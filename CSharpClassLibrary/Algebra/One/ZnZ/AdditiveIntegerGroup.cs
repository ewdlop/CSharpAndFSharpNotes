namespace CSharpClassLibrary.Algebra.One.ZnZ
{
    public class AdditiveIntegerGroup : AdditiveIntegerMonoid, IGroup<long>
    {
        public long Inverse(long a) => -a;
    }

}
