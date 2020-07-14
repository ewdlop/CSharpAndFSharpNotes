using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary.IIterator
{
    public interface IIterator<T> : IDisposable
    {
        bool HasNext { get; }
        T Next();
        void Remove();
    }
}
