namespace CSharpClassLibrary.Algebra.AntimatroidModified.RealNumber
{
    public class RealRingoid : IRingoid<double, AdditiveRealGroupoid, MultiplicativeRealGroupoid>
    {
        public AdditiveRealGroupoid Addition { get; init; }
        public MultiplicativeRealGroupoid Multiplication { get; init; }

        public RealRingoid()
        {
            Addition = new();
            Multiplication = new();
        }

        public double Distribute(double a, double b) => Multiplication.Operation(a, b);
    }
}
