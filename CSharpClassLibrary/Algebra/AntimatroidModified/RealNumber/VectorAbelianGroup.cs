namespace CSharpClassLibrary.Algebra.AntimatroidModified.RealNumber
{
    public class VectorAbelianGroup<T> : IAbelianGroup<Vector<T>>
    {
        //public IAbelianGroup<T> AbelianGroup { get; init; }
        ////public VectorAbelianGroup(IAbelianGroup<T> abelianGroup)
        ////{
        ////    AbelianGroup = abelianGroup;
        ////}

        public Vector<T> Identity => throw new System.NotImplementedException();

        public Vector<T> Inverse(Vector<T> t)
        {
            throw new System.NotImplementedException();
        }

        public Vector<T> Operation(Vector<T> a, Vector<T> b)
        {
            throw new System.NotImplementedException();
        }
    }
}
