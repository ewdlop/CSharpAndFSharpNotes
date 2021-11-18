namespace CSharpClassLibrary.Algebra.AntimatroidModified.RealNumber
{
    public class RealSemiring : RealRingoid, ISemiring<double, AdditiverRealMonoid, MultiplicativeRealMonoid>
    {
        public new AdditiverRealMonoid Addition { get; init; }
        public new MultiplicativeRealMonoid Multiplication { get; init; }

        public RealSemiring()
        {
            Addition = new();
            Multiplication = new();
        }
    }
}
