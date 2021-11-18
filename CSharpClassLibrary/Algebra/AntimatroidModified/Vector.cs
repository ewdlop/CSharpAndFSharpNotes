namespace CSharpClassLibrary.Algebra.AntimatroidModified
{
    public class Vector<T>
    {
        private T[] vector;

        public int Dimension
        {
            get { return vector.Length; }
        }

        public T this[int n]
        {
            get { return vector[n]; }
            set { vector[n] = value; }
        }

        public Vector()
        {
            vector = new T[2];
        }
    }
}
