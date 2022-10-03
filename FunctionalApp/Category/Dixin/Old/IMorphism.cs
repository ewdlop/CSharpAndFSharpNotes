using System.Diagnostics.Contracts;

namespace CSharpClassLibrary.Category.Dixin.Old;

public interface IMorphism<in TSource, out TResult, out TCategory> where TCategory : ICategory<TCategory>
{
    [Pure]
    TCategory Category { get; }

    [Pure]
    TResult Invoke(TSource source);
}
