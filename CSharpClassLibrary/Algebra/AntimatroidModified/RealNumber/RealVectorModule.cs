namespace CSharpClassLibrary.Algebra.AntimatroidModified.RealNumber
{
    public class RealVectorModule : IModule<double, 
        Vector<double>, 
        RealRing, 
        AdditiveRealGroup,
        MultiplicativeRealMonoid, 
        VectorAbelianGroup<double>>
    {
        public RealRing Scalar
        {
            get;
            private set;
        }

        public VectorAbelianGroup<double> Vector
        {
            get;
            private set;
        }

        public RealVectorModule()
        {
            Scalar = new RealRing();
            Vector = new VectorAbelianGroup<double>(/*new AdditiveRealAbelianGroup()*/);
        }

        public Vector<double> Distribute(double t, Vector<double> r)
        {
            Vector<double> c = new Vector<double>();
            for (int i = 0; i < c.Dimension; i++)
                c[i] = Scalar.Multiplication.Operation(t, r[i]);
            return c;
        }
    }
}
