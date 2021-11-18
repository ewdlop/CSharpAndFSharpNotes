namespace CSharpClassLibrary.Algebra.AntimatroidModified.RealNumber
{
    public class MultiplicativeRealMonoid : MultiplicativeRealSemigroup, IMonoid<double>
    {
        public double Identity => 1.0;
    }
}
