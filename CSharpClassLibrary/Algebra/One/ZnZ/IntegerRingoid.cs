namespace CSharpClassLibrary.Algebra.One.ZnZ
{

    public class IntegerRingoid : IRingoid<long, AdditiveIntegerGroupoid, MultiplicativeIntegerGroupoid>
    {
        public AdditiveIntegerGroupoid Addition { get; init; }
        public MultiplicativeIntegerGroupoid Multiplication { get; init; }

        public IntegerRingoid()
        {
            Addition = new ();
            Multiplication = new ();
        }

        public long Distribute(long a, long b) => Multiplication.Operation(a, b);
    }
}
