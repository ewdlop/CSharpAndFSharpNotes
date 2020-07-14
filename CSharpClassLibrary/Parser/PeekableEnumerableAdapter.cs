using CSharpClassLibrary.IIterator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//https://stackoverflow.com/questions/1273001/is-there-a-hasnext-method-for-an-ienumerator

namespace CSharpClassLibrary
{
    public sealed class PeekableEnumerableAdapter<T> : IIterable<T>, IIterator<T>
    {
        public bool FetchedNext { get; private set; }
        public bool NextAvailable { get; private set; }
        public IEnumerable<T> Enumerable { get; }
        public IEnumerator<T> Enumerator { get; }
        public int CacheSize { get; } = 10;
        public Queue<T> Queue { get; } = new Queue<T>();
        public Stack<T> Stack { get; } = new Stack<T>();
        public T EndToken { get; private set; }
        public T NextObject { get; private set; }

        public PeekableEnumerableAdapter(IEnumerable<T> enumerable)
        {
            Enumerable = enumerable;
            Enumerator = enumerable.GetEnumerator();
        }

        public PeekableEnumerableAdapter(IEnumerable<T> enumerable, T endToken)
        {
            Enumerable = enumerable;
            Enumerator = enumerable.GetEnumerator();
            EndToken = endToken;
        }

        public PeekableEnumerableAdapter(IEnumerator<T> enumerator)
        {
            Enumerator = enumerator;
        }

        public IIterator<T> Iterator()
        {
            return new PeekableEnumerableAdapter<T>(Enumerable.GetEnumerator());
        }

        public bool HasNext
        {
            get
            {
                if (EndToken != null || Stack.Count > 0) return true;
                CheckNext();
                return NextAvailable;
            }
        }


        public T Peek()
        {
            if (Stack.Count > 0)
            {
                return Stack.Peek();
            }
            if (!HasNext)
            {
                return EndToken;
            }
            T val = Next();
            PutItBack();
            return val;
        }

        public T Next()
        {
            if (Stack.Count > 0)
            {
                T NextObject = Stack.Pop();
                while (Queue.Count > CacheSize - 1)
                {
                    Queue.Dequeue();
                }
                Queue.Enqueue(NextObject);
                return NextObject;
            } 
            else
            {
                CheckNext();
                if (!NextAvailable)
                {
                    T tmp = EndToken;
                    EndToken = default;
                    return tmp;
                }
                while (Queue.Count > CacheSize - 1)
                {
                    Queue.Dequeue();
                }
                Queue.Enqueue(NextObject);
                FetchedNext = false;
                return NextObject;
            }
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

        public void Remove()
        {
            throw new NotSupportedException();
        }

        public void PutItBack()
        {
            if (Queue.Count > 0)
            {
                Queue.Reverse();
                Stack.Push(Queue.Dequeue());
                Queue.Reverse();
            }
        }

        public void Dispose()
        {
            Enumerator.Dispose();
        }
    }
}
