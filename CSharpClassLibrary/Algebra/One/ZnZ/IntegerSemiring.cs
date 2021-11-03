namespace CSharpClassLibrary.Algebra.One.ZnZ
{

    public class IntegerSemiring : IntegerRingoid, ISemiring<long, AdditiveIntegerMonoid, MultiplicativeIntegerMonoid>
    {
        public new AdditiveIntegerMonoid Addition { get; init; }
        public new MultiplicativeIntegerMonoid Multiplication { get; init; }

        public IntegerSemiring()
        {
            Addition = new();
            Multiplication = new();
        }
    }
}
