using System.Diagnostics.CodeAnalysis;

namespace CSharpClassLibrary.CSharp9.UnconstrainedType
{
    public abstract class A
    {
        [return: MaybeNull] public abstract T F1<T>();
        public abstract void F2<T>([AllowNull] T t);
    }

    public class B : A
    {
        public override T? F1<T>() where T : default
        {
            return default;
        } 
        public override void F2<T>(T? t) where T : default { } // matches A.F2<T>()
    }

    class A1
    {
        public virtual void F1<T>(T? t) where T : struct { }
        public virtual void F1<T>(T? t) where T : class { }
    }

    class B1 : A1
    {
        public override void F1<T>(T? t) /*where T : struct*/ { }
        public override void F1<T>(T? t) where T : class { }
    }

    class A2
    {
        public virtual void F2<T>(T? t) where T : struct { }
        public virtual void F2<T>(T? t) { }
    }

    class B2 : A2
    {
        public override void F2<T>(T? t) /*where T : struct*/ { }
        public override void F2<T>(T? t) where T : default { }
    }
}
