namespace CSharpClassLibrary.Algebra.AntimatroidModified.RealNumber
{
    public class AdditiveRealGroupoid : IGroupoid<double>
    {
        public double Operation(double a, double b) => a + b;
    }
}
