namespace CSharpClassLibrary.Algebra.Static
{
    interface IMonid<T>
    {
        static T Zero { get; }
        //static T operator +(T t1, T t2);
    }

    //public static T AndAll<T>(T[] tes) where T: IMonid<T>
    //{
    //    //T result = ((IMonid<T>)T).Zero;
    //    foreach(T t in ts) { result += t; }
    //    return result;
    //}

    struct Int32 : IMonid<int>
    {
        public static int Zero  => 0;
    }
}
