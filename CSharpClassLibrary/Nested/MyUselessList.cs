using System.Collections;
using System.Collections.Generic;

namespace CSharpClassLibrary.Nested;

class MyUselessList : IEnumerable<int>
{
    // ...
    private List<int> internalList;
    private class UselessListEnumerator : IEnumerator<int>
    {
        private MyUselessList obj;
        public UselessListEnumerator(MyUselessList o)
        {
            obj = o;
        }
        private int currentIndex = -1;
        public int Current
        {
            get { return obj.internalList[currentIndex]; }
        }

        object IEnumerator.Current => throw new System.NotImplementedException();

        public bool MoveNext()
        {
            return ++currentIndex < obj.internalList.Count;
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
    public IEnumerator<int> GetEnumerator()
    {
        return new UselessListEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
