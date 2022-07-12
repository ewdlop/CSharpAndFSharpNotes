using System;
using System.Linq;

namespace BitWiseOperation;

public static class IntegerExtension
{
    public static int ConvertToDate(int year, int month, int day)
    {
        return (((year << 4) + month) << 5) + day;
    }

    public static (int year, int month, int day) ToDate(this int date)
    {
        return (date>>9, (date>>5)%16,date%32);
    }

    public static int GetRuler(this int x)
    {
        if (x == 0) throw new NotDefineException(null); //infinity
        int k = 0;
        int max = k;
        int powerOfTwo = 2 << k;
        while (powerOfTwo<=x)
        {
            if (x % powerOfTwo == 0) max = k;
            powerOfTwo = 2 << k++;
        }
        return max;
    }
    /// <summary>
    /// x is power of 2
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static int GetRuler2(this int x) //(a01^a10^b)
    {
        int rho = 0;
        int y = x & (~x + 1); //x&-x
        int k = 0;
        int d = 32;
        while (k < d)
        {
            if ((y & Mask.MagicMask(k, d)) == 0) rho += 1 << k;
            k++;
        }
        return rho;
    }

    private readonly static byte[] DecodeTalbe = new byte[64]
    {
        00, 01, 56, 02, 57, 49, 28, 03, 61, 58, 42, 50, 38, 29, 17, 04,
        62, 47, 59, 36, 45, 43, 51, 22, 53, 39, 33, 30, 24, 18, 12, 05,
        63, 55, 48, 27, 60, 41, 37, 16, 46, 35, 44, 21, 52, 32, 23, 11,
        54, 26, 40, 15, 34, 20, 31, 10, 25, 14, 19, 09, 13, 08, 07, 06
    };

    public static int GetRuler3(this int x)
    {
        return DecodeTalbe[(0x03f79d71b4ca8b09 * x & (~x + 1) % long.MaxValue) >> 58];
    }

    public static int LeftMostBitPlace1(this int x) //lambda(2x)=lambda(2x+1)=lambda(x)+1
    {
        int lambda = 0;
        ulong y = (ulong)x;
        int k = 63;
        while(k >= 0)
        {
            if(y >> (2 << k - 1) != 0)
            {
                lambda += 2 << k - 1;
                y >>= 2 << k - 1;
            }
            k--;
        }
        //need short able
        return lambda;
    }

    public static int ExtractLeftMostBit(this int x)
    {
        if (x <= 0) throw new NotDefineException(null);
        int y = x;
        int k = 0;
        int d = 32;
        while(k < d)
        {
            y |= y >> (1 << k);
            k++;
        }
        return y - (y >> 1);
    }

    public static int ExtractLeftMostBit2(this int x)
    {
        if (x <= 0) throw new NotDefineException(null);
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;
        return x - (x >> 1);
    }

    private static readonly int[] LookupTable =
        Enumerable.Range(0, 256).Select(SidewaysSum).ToArray();

    public static int SidewaysSum(this int value)
    {
        int count = 0;
        for (int i = 0; i < 32; i++)
        {
            count += (value >> i) & 1;
        }
        return count;
    }
}
