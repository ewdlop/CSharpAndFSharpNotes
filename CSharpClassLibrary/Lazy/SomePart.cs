using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpClassLibrary.Lazy
{
    public class SomePart : Lazy<Task<SlowPart>>
    {
        public SomePart(OtherPart eagerPart, Func<Task<SlowPart>> lazyPartFactory)
            : base(() => Task.Run(lazyPartFactory))
        {
            EagerPart = eagerPart;
        }

        public OtherPart EagerPart { get; }
        public TaskAwaiter<SlowPart> GetAwaiter() => Value.GetAwaiter();
    }

    public class OtherPart
    {
    }

    public class SlowPart
    {
    }
}