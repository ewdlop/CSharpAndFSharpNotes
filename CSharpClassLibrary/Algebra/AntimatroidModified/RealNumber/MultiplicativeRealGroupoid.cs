namespace CSharpClassLibrary.Algebra.AntimatroidModified.RealNumber
{
    public class MultiplicativeRealGroupoid : IGroupoid<double>
    {
        public double Operation(double a, double b) => a * b;
    }
}
