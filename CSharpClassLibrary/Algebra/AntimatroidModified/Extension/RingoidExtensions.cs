using System.Collections.Generic;
using System.Linq;

namespace CSharpClassLibrary.Algebra.AntimatroidModified;

public static class RingoidExtensions
{
    //static public T Count<T, R, A, M>(this IEnumerable<R> E, IRingWithUnity<T, A, M> r)
    //    where A : IGroup<T>
    //    where M : IMonoid<T>
    //{

    //    return E
    //        .Map((x) => r.Multiplication.Identity)
    //        .Sum(r.Addition);
    //}

    //static public T Mean<T, A, M>(this IEnumerable<T> E, IDivisionRing<T, A, M> r)
    //    where A : IGroup<T>
    //    where M : IGroup<T>
    //{

    //    return r.Multiplication.Operation(
    //        r.Multiplication.Inverse(
    //            E.Count(r)
    //        ),
    //        E.Sum(r.Addition)
    //    );
    //}

    //static public T Variance<T, A, M>(this IEnumerable<T> E, IDivisionRing<T, A, M> r)
    //    where A : IGroup<T>
    //    where M : IGroup<T>
    //{

    //    T average = E.Mean(r);

    //    return r.Multiplication.Operation(
    //        r.Multiplication.Inverse(
    //            E.Count(r)
    //        ),
    //        E
    //            .Map((x) => r.Addition.Operation(x, r.Addition.Inverse(average)))
    //            .Map((x) => r.Multiplication.Operation(x, x))
    //            .Sum(r.Addition)
    //    );
    //}
}