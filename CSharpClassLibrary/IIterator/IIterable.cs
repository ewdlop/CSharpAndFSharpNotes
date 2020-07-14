using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary.IIterator
{
    public interface IIterable<T>
    {
        IIterator<T> Iterator();
    }
}
