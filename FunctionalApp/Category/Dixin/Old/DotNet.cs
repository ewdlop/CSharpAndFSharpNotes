using System.Diagnostics.Contracts;

namespace CSharpClassLibrary.Category.Dixin.Old;

public class DotNet : ICategory<DotNet>
{
    [Pure]
    public IMorphism<TObject, TObject, DotNet> Id<TObject>
        () => new DotNetMorphism<TObject, TObject>(@object => @object);

    [Pure]
    public IMorphism<TSource, TResult, DotNet> o<TSource, TMiddle, TResult>
        (IMorphism<TMiddle, TResult, DotNet> m2, IMorphism<TSource, TMiddle, DotNet> m1) =>
            new DotNetMorphism<TSource, TResult>(@object => m2.Invoke(m1.Invoke(@object)));

    private DotNet()
    {
    }

    public static DotNet Category { [Pure] get; } = new DotNet();
}
