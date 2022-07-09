using System;

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
        if (x == 0) throw new NotDefineException(); //infinity
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
}
