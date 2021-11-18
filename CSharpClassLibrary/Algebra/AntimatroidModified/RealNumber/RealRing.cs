namespace CSharpClassLibrary.Algebra.AntimatroidModified.RealNumber
{
    public class RealRing : RealSemiring, IRing<double, AdditiveRealGroup, MultiplicativeRealMonoid>
    {
        public new AdditiveRealGroup Addition { get; init; }

        public RealRing()
        {
            Addition = new();
        }
    }
}
