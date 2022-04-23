using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BitWiseOperation;

public unsafe class IntegerByteData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return Enumerable.Range(0, 256).Select(i =>
        {
            //byte bits = ((byte*) &i)[0];
            return new object[] {Convert.ToByte(i), Convert.ToByte(i) };
        }).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public static class Mask
{
    public static double MagicMask(uint k, uint d)//2-adic fraction -1/(2^(2k+1)+)
    {
        return (Math.Pow(2, Math.Pow(2, d)) - 1) / (Math.Pow(2, Math.Pow(2, k)) + 1);
    }
}

public static class PrimeNumber
{
    public static readonly ulong[] P = new ulong[8] {0, 0, 0, 0, 0, 0, 0, 0};
    public static readonly ulong[] Q = P.Reverse().ToArray();
    public static bool IsPrimeP(int x)
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
    public static bool IsPrimeQ(int x)
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
        if (x == 0) throw new NotDefineException();
        int k = 0;
        int max = k;
        int powerOfTwo = (int)Math.Pow(2, k);
        while (powerOfTwo<=x)
        {
            if (x % powerOfTwo == 0) max = k;
            powerOfTwo = (int)Math.Pow(2, ++k);
        }
        return max;
    }

    //public static int GetRuler2(this int x)
    //{
    //    int rho = 0;
    //    int y = x & (~x + 1);
    //    uint k = 0;
    //    uint d = 64;
    //    while (k <= 64)
    //    {
    //        if(y&Mask.MagicMask(k, d).To2AdicRepresentation())==0) rho += (int)Math.Pow(2, k);
    //        k++;
    //    }
    //    return rho;
    //}
}

public class NotDefineException : Exception
{
}

public static class ByteArrayExtension
{
    //C#11?
    //public static byte[] operator -(this byte[] y) => default;
    //public static Byte operator -(this Byte y) => default;
    //public static Byte[] operator -(Byte[] y) => default;

    public static byte ToNegation(this byte x)
    {
        return (byte)(~x + 1);
    }
    public static byte Substract(this byte x,byte y)
    {
        return (byte)~(~x + y);
    }
    public static bool Contains(this byte[] y, byte[] x)
    {
        return y.Select((_, k) => (x[k] & y[k]) == x[k])
            .All(subContains=> subContains);
    }
}

public class UnitTest1
{
    [Theory]
    [ClassData(typeof(IntegerByteData))]
    public void And(byte x, byte y)
    {
        Assert.Equal(0, x & 0);
        Assert.Equal(x, x & x);
        Assert.Equal(x, x & -1);
        Assert.Equal(0, x & ~x);
        Assert.Equal(~x | ~y, ~(x & y));
    }

    [Theory]
    [ClassData(typeof(IntegerByteData))]
    public void Or(byte x, byte y)
    {
        Assert.Equal(x, x | 0);
        Assert.Equal(x, x | x);
        Assert.Equal(-1, x | -1);
        Assert.Equal(-1, x | ~x);
        Assert.Equal(~x & ~y, ~(x | y));
    }

    [Theory]
    [ClassData(typeof(IntegerByteData))]
    public void Xor(byte x, byte y)
    {
        Assert.Equal(x, x^ 0);
        Assert.Equal(0, x^ x);
        Assert.Equal(~x, x^ -1);
        Assert.Equal(-1, x^ ~x);
        Assert.Equal(~x ^ y, ~(x ^ y));
        Assert.Equal(x ^ ~y, ~(x ^ y));
    }

    [Theory]
    [ClassData(typeof(IntegerByteData))]
    public void Shift(byte x, byte y)
    {
        const int power = 2;
        Assert.Equal(Math.Round(Convert.ToDecimal((int)Math.Pow(2,power)*x)),Convert.ToDecimal(x << power));
        Assert.Equal(Math.Round(Convert.ToDecimal(x / (int)Math.Pow(2, power))), Convert.ToDecimal(x >> power));
        Assert.Equal((x&y)<< power, (x<< power) &(y<< power));
    }


    [Theory]
    [ClassData(typeof(IntegerByteData))]
    public void Mod(byte x, byte y)
    {
        const int power = 2;
        Assert.Equal(x% (int)Math.Pow(2, power), x&((int)Math.Pow(2, power)-1));
    }

    [Theory]
    [InlineData(9, 0)]
    [InlineData(8, 3)]
    [InlineData(10,1)]
    [InlineData(20,2)]
    public void Ruler(int x, int y)
    {
        Assert.Equal(y, x.GetRuler());
        Assert.Equal(x.GetRuler()+1,(2*x).GetRuler());
        Assert.Equal((x ^ y).GetRuler(), (x - y).GetRuler());
    }
}
