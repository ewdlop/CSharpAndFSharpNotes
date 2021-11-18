namespace CSharpClassLibrary.Algebra.AntimatroidModified.D8
{

    public class D8SymmetryGroup : D8SymmetryMonoid, IGroup<D8Symmetry>
    {
        public D8Symmetry Inverse(D8Symmetry a)
        {
            return a switch
            {
                D8Symmetry.Rot000 => D8Symmetry.Rot000,
                D8Symmetry.Rot090 or D8Symmetry.Rot180 => D8Symmetry.Rot270,
                D8Symmetry.Rot270 => D8Symmetry.Rot090,
                D8Symmetry.RefVer => D8Symmetry.RefVer,
                D8Symmetry.RefDes => D8Symmetry.RefAsc,
                D8Symmetry.RefHoz => D8Symmetry.RefHoz,
                D8Symmetry.RefAsc => D8Symmetry.RefDes,
                _ => throw new System.NotImplementedException(),
            };
        }

    }
}
