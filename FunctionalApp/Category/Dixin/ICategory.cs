using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpClassLibrary.Category.Dixin;

public interface ICategory<TObject, TMorphism>
{
    IEnumerable<TObject> Objects { get; }

    TMorphism Compose(TMorphism morphism2, TMorphism morphism1);

    TMorphism Id(TObject @object);
}

public partial class DotNetCategory : ICategory<Type, Delegate>
{
    public IEnumerable<Type> Objects =>
        SelfAndReferences(typeof(DotNetCategory).Assembly)
            .SelectMany(assembly => assembly.GetExportedTypes());

    public Delegate Compose(Delegate morphism2, Delegate morphism1) =>
        // return (Func<TSource, TResult>)Functions.Compose<TSource, TMiddle, TResult>(
        //    (Func<TMiddle, TResult>)morphism2, (Func<TSource, TMiddle>)morphism1);
        (Delegate)typeof(Functions).GetMethod(nameof(Functions.o))
            .MakeGenericMethod( // TSource, TMiddle, TResult.
                morphism1.Method.GetParameters().Single().ParameterType,
                morphism1.Method.ReturnType,
                morphism2.Method.ReturnType)
            .Invoke(null, new object[] { morphism2, morphism1 });

    public Delegate Id(Type @object) => // Functions.Id<TSource>
        typeof(Functions).GetMethod(nameof(Functions.Id)).MakeGenericMethod(@object)
            .CreateDelegate(typeof(Func<,>).MakeGenericType(@object, @object));

    private static IEnumerable<Assembly> SelfAndReferences(
        Assembly self, HashSet<Assembly> selfAndReferences = null)
    {
        selfAndReferences = selfAndReferences ?? new HashSet<Assembly>();
        if (selfAndReferences.Add(self))
        {
            Array.ForEach(self.GetReferencedAssemblies(), reference =>
                SelfAndReferences(Assembly.Load(reference), selfAndReferences));
            return selfAndReferences;
        }
        return Enumerable.Empty<Assembly>(); // Circular or duplicate reference.
    }
}