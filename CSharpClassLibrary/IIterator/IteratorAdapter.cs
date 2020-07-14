using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary.IIterator
{
    public sealed class IterableAdapter<T> : IEnumerable<T>
    {
        public IIterable<T> Iterable { get; }
        public IterableAdapter(IIterable<T> iterable)
        {
            Iterable = iterable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new IteratorAdapter<T>(Iterable.Iterator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class IteratorAdapter<T> : IEnumerator<T>
    {
        public IIterator<T> Iterator { get; }
        public bool GotCurrent { get; private set; }
        public T CurrentObject { get; private set; }

        public IteratorAdapter(IIterator<T> iterator)
        {
            Iterator = iterator;
        }

        public T Current
        {
            get
            {
                if (!GotCurrent)
                {
                    throw new InvalidOperationException();
                }
                return CurrentObject;
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            GotCurrent = Iterator.HasNext;
            if (GotCurrent) {
                CurrentObject = Iterator.Next();
            }
            return GotCurrent;
        }

        public void Reset() => throw new NotSupportedException();

        public void Dispose() => Iterator.Dispose();
    }
}
