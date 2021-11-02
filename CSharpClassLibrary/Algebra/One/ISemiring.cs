namespace CSharpClassLibrary.Algebra.One
{
    public interface ISemiring<T, A, M> : IRingoid<T, A, M>
        where A : IMonoid<T>
        where M : IMonoid<T>
    {

    }

    //Dihedral Group 8
    public class SymmetryGroupoid : IGroupoid<Symmetry>
    {
        public Symmetry Operation(Symmetry s1, Symmetry s2)
        {
            var tuple = (s1, s2);
            return tuple switch
            {
                (Symmetry.Rot000, _) => s2,
                (Symmetry.Rot090, Symmetry.Rot090) => Symmetry.Rot180,
                (Symmetry.Rot090, Symmetry.Rot180) => Symmetry.Rot270,
                (Symmetry.Rot090, Symmetry.Rot270) => Symmetry.Rot000,
                (Symmetry.Rot090, Symmetry.RefVer) => Symmetry.RefAsc,
                (Symmetry.Rot090, Symmetry.RefDes) => Symmetry.RefHoz,
                (Symmetry.Rot090, Symmetry.RefHoz) => Symmetry.RefAsc,
                (Symmetry.Rot090, Symmetry.RefAsc) => Symmetry.RefVer,
                (Symmetry.Rot180, Symmetry.Rot090) => Symmetry.Rot270,
                (Symmetry.Rot180, Symmetry.Rot180) => Symmetry.Rot000,
                (Symmetry.Rot180, Symmetry.Rot270) => Symmetry.Rot090,
                (Symmetry.Rot180, Symmetry.RefVer) => Symmetry.RefHoz,
                (Symmetry.Rot180, Symmetry.RefDes) => Symmetry.RefAsc,
                (Symmetry.Rot180, Symmetry.RefHoz) => Symmetry.RefVer,
                (Symmetry.Rot180, Symmetry.RefAsc) => Symmetry.RefDes,
                (Symmetry.Rot270, Symmetry.Rot090) => Symmetry.Rot000,
                (Symmetry.Rot270, Symmetry.Rot180) => Symmetry.Rot090,
                (Symmetry.Rot270, Symmetry.Rot270) => Symmetry.Rot180,
                (Symmetry.Rot270, Symmetry.RefVer) => Symmetry.RefDes,
                (Symmetry.Rot270, Symmetry.RefDes) => Symmetry.RefVer,
                (Symmetry.Rot270, Symmetry.RefHoz) => Symmetry.RefDes,
                (Symmetry.Rot270, Symmetry.RefAsc) => Symmetry.RefHoz,
                //(Symmetry.RefDes, Symmetry.Rot090) => Symmetry.RefHoz,
                //(Symmetry.RefDes, Symmetry.Rot180) => Symmetry.RefAsc,
                //(Symmetry.RefDes, Symmetry.Rot270) => Symmetry.RefVer,
                //(Symmetry.RefDes, Symmetry.RefVer) => Symmetry.Rot270,
                //(Symmetry.RefDes, Symmetry.RefDes) => Symmetry.RefHoz,
                //(Symmetry.RefDes, Symmetry.RefHoz) => Symmetry.RefAsc,
                //(Symmetry.RefDes, Symmetry.RefAsc) => Symmetry.RefAsc,
                (Symmetry.RefVer, _) => Operation(s2, Symmetry.RefVer),
                (_, Symmetry.Rot000) => s1,

            };
        }
    }
    public class SymmetrySemigroup : SymmetryGroupoid, ISemigroup<Symmetry>
    {

    }

    public class SymmetryMonoid : SymmetryGroupoid,IMonoid<Symmetry>
    {
        public Symmetry Identity
        {
            get { return Symmetry.Rot000; }
        }
    }

    public class SymmetryGroup : SymmetryMonoid, IGroup<Symmetry>
    {
        public Symmetry Inverse(Symmetry a)
        {
            switch (a)
            {
                case Symmetry.Rot000:
                    return Symmetry.Rot000;
                case Symmetry.Rot090:
                case Symmetry.Rot180:
                    return Symmetry.Rot270;
                case Symmetry.Rot270:
                    return Symmetry.Rot090;
                case Symmetry.RefVer:
                    return Symmetry.RefVer;
                case Symmetry.RefDes:
                    return Symmetry.RefAsc;
                case Symmetry.RefHoz:
                    return Symmetry.RefHoz;
                case Symmetry.RefAsc:
                    return Symmetry.RefDes;
            }

            throw new System.NotImplementedException();
        }

    }
}
