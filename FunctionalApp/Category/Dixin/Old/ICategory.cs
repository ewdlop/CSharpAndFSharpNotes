using System.Diagnostics.Contracts;

namespace CSharpClassLibrary.Category.Dixin.Old;

public interface ICategory<TCategory> where TCategory : ICategory<TCategory>
{
    // o = (m2, m1) -> composition
    [Pure]
    IMorphism<TSource, TResult, TCategory> o<TSource, TMiddle, TResult>(
        IMorphism<TMiddle, TResult, TCategory> m2, IMorphism<TSource, TMiddle, TCategory> m1);

    [Pure]
    IMorphism<TObject, TObject, TCategory> Id<TObject>();
}
