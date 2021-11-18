namespace CSharpClassLibrary.Algebra.AntimatroidModified.ZnZ
{
    public class AdditiveIntegerGroup : AdditiveIntegerMonoid, IGroup<long>
    {
        public long Inverse(long a) => -a;
    }

}
