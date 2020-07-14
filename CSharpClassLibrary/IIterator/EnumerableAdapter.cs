using System;
using System.Collections.Generic;

namespace CSharpClassLibrary.IIterator
{
    public sealed class EnumerableAdapter<T> : IIterable<T>
    {
        public IEnumerable<T> Enumerable { get; }
        public EnumerableAdapter(IEnumerable<T> enumerable) => Enumerable = enumerable;

        public IIterator<T> Iterator() => new EnumeratorAdapter<T>(Enumerable.GetEnumerator());
    }

    public sealed class EnumeratorAdapter<T> : IIterator<T>
    {
        public IEnumerator<T> Enumerator { get; }
        public bool FetchedNext { get; private set; }
        public bool NextAvailable { get; private set; }
        public T NextObject { get; private set; }
        public EnumeratorAdapter(IEnumerator<T> enumerator) => Enumerator = enumerator;

        public bool HasNext
        {
            get
            {
                CheckNext();
                return NextAvailable;
            }
        }

        public T Next()
        {
            CheckNext();
            if (!NextAvailable)
            {
                throw new InvalidOperationException();
            }
            FetchedNext = false;
            return NextObject;
        }

        private void CheckNext()
        {
            if (!FetchedNext)
            {
                NextAvailable = Enumerator.MoveNext();
                if (NextAvailable)
                {
                    NextObject = Enumerator.Current;
                }
                FetchedNext = true;
            }
        }

        public void Remove() => throw new NotSupportedException();

        public void Dispose() => Enumerator.Dispose();
    }
}
