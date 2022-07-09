using System;
using System.Linq;

namespace BitWiseOperation;

public static class PrimeNumber
{
    public static readonly ulong[] P = new ulong[8] {0, 0, 0, 0, 0, 0, 0, 0};
    public static readonly ulong[] Q = P.Reverse().ToArray();
    public static bool IsPrimeBigEndian(int x)
    {
        if (x switch {
                <=0 => false,
                2 => true,
                >= 1024 => throw new NotSupportedException("Does not work for integer greater than equal to 1024"),
                _ => true
            })
        {
            if(x == 2) return true;
            int k = (x - 1) / 2;
            ulong p = P[k / 64]<<(k & 63);
            return p %((ulong)2 << 63) < 0;
        }
        else
        {
            return false;
        }
    }
    public static bool IsPrimeLittleEndian(int x)
    {
        if (x switch
            {
                <= 0 => false,
                2 => true,
                >= 1024 => throw new NotSupportedException("Does not work for integer greater than equal to 1024"),
                _ => true
            })
        {
            if (x == 2) return true;
            int k = (x - 1) / 2;
            ulong p = Q[k / 64] >> (k & 63);
            return p % 2 == 1;
        }
        else
        {
            return false;
        }
    }
}
