using System;

namespace BitWiseOperation;

public static class Mask
{
    /// <summary>
    /// return truncated of infinite 2-adic fraction -1/(2^(2k+1)+).  0 less than or equla to k less than d
    /// </summary>
    /// <param name="k"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static int MagicMask(uint k, uint d)
    {
        if(k == d || k > d) throw new ApplicationException("k must be less than d");
        int k1 = Convert.ToInt32(k);
        int d1 = Convert.ToInt32(d);
        return (1 << (1 << d1) - 1) / (1<< (1 << k1) + 1);
    }

    /// <summary>
    /// return truncated of infinite 2-adic fraction -1/(2^(2k+1)+). 0 less than or equla to k less than d
    /// </summary>
    /// <param name="k"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static int MagicMask(int k, int d)
    {
        if(k < 0) throw new ApplicationException("k must be great than or equal to 0");
        if(d <= 0) throw new ApplicationException("d must be greater than 0");
        if(k > d) throw new ApplicationException("k must be less than d");
        return (1 << (1 << d) - 1) / (1 << (1 << k) + 1);
    }

    public static int Divide(this int dividend, int divisor)
    {
        if (divisor == 0 || dividend == int.MinValue && divisor == -1)//overflow check
        {
            return int.MaxValue;
        }
        
        int sign = 1;
        if (dividend < 0 ^ divisor < 0)
        { 
            sign = -1; //XOR if either is negative, but not both
        }
        
        // Convert to Long or else abs(-2147483648) overflows
        long numerator = Math.Abs((long)dividend);
        long denominator = Math.Abs((long)divisor);
        int quotient = 0;

        while (numerator >= denominator)
        {
            long denCopy = denominator;
            int multiple = 1;
            while (numerator >= denCopy << 1)
            {
                denCopy <<= 1; // << is multiply by 2
                multiple <<= 1;
            }
            numerator -= denCopy;
            quotient += multiple;
        }
        return quotient * sign;
    }
}
