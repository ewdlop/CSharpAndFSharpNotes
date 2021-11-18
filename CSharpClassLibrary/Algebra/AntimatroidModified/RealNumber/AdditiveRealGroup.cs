namespace CSharpClassLibrary.Algebra.AntimatroidModified.RealNumber
{
    public class AdditiveRealGroup : AdditiverRealMonoid, IGroup<double>
    {
        public double Inverse(double a) => -a;
    }
}
