namespace CSharpClassLibrary.Algebra.One.ZnZ
{

    public class IntegerRingWithUnity : IntegerRing, IRingWithUnity<long, AdditiveIntegerGroup, MultiplicativeIntegerMonoid>
    {
        public new MultiplicativeIntegerMonoid Multiplication { get; init; }

        public IntegerRingWithUnity()
        {
            Multiplication = new();
        }
    }
}
