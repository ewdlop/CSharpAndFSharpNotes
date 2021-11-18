namespace CSharpClassLibrary.Algebra.AntimatroidModified.D8
{

    public class D8SymmetryMonoid : D8SymmetryGroupoid,IMonoid<D8Symmetry>
    {
        public D8Symmetry Identity => D8Symmetry.Rot000;
    }
}
