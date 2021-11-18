namespace CSharpClassLibrary.Algebra.One.ZnZ
{
    public record IntegerRingWithUnity : IntegerRing, IRingWithUnity<long, AdditiveIntegerGroup, MultiplicativeIntegerMonoid>
    {
        public new MultiplicativeIntegerMonoid Multiplication { get; init; }

        public IntegerRingWithUnity()
        {
            Multiplication = new();
        }
    }
}
