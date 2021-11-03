namespace CSharpClassLibrary.Algebra.One.ZnZ
{

    public class IntegerRing : IntegerSemiring, IRing<long, AdditiveIntegerGroup, MultiplicativeIntegerMonoid>
    {
        public new AdditiveIntegerGroup Addition { get; init; }

        public IntegerRing()
        {
            Addition = new ();
        }
    }


}
