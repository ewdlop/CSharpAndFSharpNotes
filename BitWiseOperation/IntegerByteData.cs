using System.Collections;
using System.Collections.Generic;

namespace BitWiseOperation;

public  class IntegerByteData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return GetEnumerable().GetEnumerator();
    }

    public static IEnumerable<object[]> GetEnumerable()
    {
        for(uint i = 0; i < 256; i++)
        {
            yield return new object[] { (byte)i, (byte)i };
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
