using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace CSharpClassLibrary
{
    public class PeekIterator<T> : IEnumerator<T>
    {
        private const int CacheSize = 10;
        public IEnumerator<T> Enumerator { get; set; }
        public Queue<T> Queue { get; set; }
        public Stack<T> Stack { get; set; }

        public T EndToken { get; set; }

        public PeekIterator(IEnumerable<T> enumerable)
        {
            Enumerator = enumerable.GetEnumerator();
        }

        public PeekIterator(IEnumerable<T> enumerable, T endToken)
        {
            Enumerator = enumerable.GetEnumerator();
            EndToken = endToken;
        }
        object IEnumerator.Current => Current();
        T IEnumerator<T>.Current => Current();
        public T Current()
        {
            T val;
            if (Stack.Count > 0)
            {
                val = Stack.Pop();
            } 
            else
            {
                if(!Enumerator.MoveNext())
                {
                    T tmp = EndToken;
                    EndToken = default;
                    return tmp;
                }
                val = Enumerator.Current;
            }
            while(Queue.Count > CacheSize - 1)
            {
                Queue.Dequeue();
            }
            Queue.Enqueue(val);
            return val;
        }


        public T Peek()
        {
            if(Stack.Count > 0)
            {
                return Stack.Peek();
            }
            if(!Enumerator.MoveNext())
            {
                return EndToken;
            }
            T val = Current();
            PutItBack();
            return val;
        }

        public void Reset()
        {
            throw new NotImplementedException();
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
        }

        public bool MoveNext()
        {
            return EndToken != null || Stack.Count > 0 || Enumerator.MoveNext();
        }
    }
}
