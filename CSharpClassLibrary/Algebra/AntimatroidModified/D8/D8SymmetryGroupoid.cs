namespace CSharpClassLibrary.Algebra.AntimatroidModified.D8
{

    public class D8SymmetryGroupoid : IGroupoid<D8Symmetry>
    {
        public D8Symmetry Operation(D8Symmetry s1, D8Symmetry s2)
        {
            //unfinished
            var tuple = (s1, s2);
            return tuple switch
            {
                (D8Symmetry.Rot000, _) => s2,
                (D8Symmetry.Rot090, D8Symmetry.Rot090) => D8Symmetry.Rot180,
                (D8Symmetry.Rot090, D8Symmetry.Rot180) => D8Symmetry.Rot270,
                (D8Symmetry.Rot090, D8Symmetry.Rot270) => D8Symmetry.Rot000,
                (D8Symmetry.Rot090, D8Symmetry.RefVer) => D8Symmetry.RefAsc,
                (D8Symmetry.Rot090, D8Symmetry.RefDes) => D8Symmetry.RefHoz,
                (D8Symmetry.Rot090, D8Symmetry.RefHoz) => D8Symmetry.RefAsc,
                (D8Symmetry.Rot090, D8Symmetry.RefAsc) => D8Symmetry.RefVer,
                (D8Symmetry.Rot180, D8Symmetry.Rot090) => D8Symmetry.Rot270,
                (D8Symmetry.Rot180, D8Symmetry.Rot180) => D8Symmetry.Rot000,
                (D8Symmetry.Rot180, D8Symmetry.Rot270) => D8Symmetry.Rot090,
                (D8Symmetry.Rot180, D8Symmetry.RefVer) => D8Symmetry.RefHoz,
                (D8Symmetry.Rot180, D8Symmetry.RefDes) => D8Symmetry.RefAsc,
                (D8Symmetry.Rot180, D8Symmetry.RefHoz) => D8Symmetry.RefVer,
                (D8Symmetry.Rot180, D8Symmetry.RefAsc) => D8Symmetry.RefDes,
                (D8Symmetry.Rot270, D8Symmetry.Rot090) => D8Symmetry.Rot000,
                (D8Symmetry.Rot270, D8Symmetry.Rot180) => D8Symmetry.Rot090,
                (D8Symmetry.Rot270, D8Symmetry.Rot270) => D8Symmetry.Rot180,
                (D8Symmetry.Rot270, D8Symmetry.RefVer) => D8Symmetry.RefDes,
                (D8Symmetry.Rot270, D8Symmetry.RefDes) => D8Symmetry.RefVer,
                (D8Symmetry.Rot270, D8Symmetry.RefHoz) => D8Symmetry.RefDes,
                (D8Symmetry.Rot270, D8Symmetry.RefAsc) => D8Symmetry.RefHoz,
                //(Symmetry.RefDes, Symmetry.Rot090) => Symmetry.RefHoz,
                //(Symmetry.RefDes, Symmetry.Rot180) => Symmetry.RefAsc,
                //(Symmetry.RefDes, Symmetry.Rot270) => Symmetry.RefVer,
                //(Symmetry.RefDes, Symmetry.RefVer) => Symmetry.Rot270,
                //(Symmetry.RefDes, Symmetry.RefDes) => Symmetry.RefHoz,
                //(Symmetry.RefDes, Symmetry.RefHoz) => Symmetry.RefAsc,
                //(Symmetry.RefDes, Symmetry.RefAsc) => Symmetry.RefAsc,
                (D8Symmetry.RefVer, _) => Operation(s2, D8Symmetry.RefVer),
                (_, D8Symmetry.Rot000) => s1,

            };
        }
    }
}
