using System;
using System.Linq;
using Xunit;

namespace BitWiseOperation;

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
        Assert.Equal(Math.Round(Convert.ToDecimal((2 << power - 1) *x)),Convert.ToDecimal(x << power));
        Assert.Equal(Math.Round(Convert.ToDecimal(x / (2 << power - 1))), Convert.ToDecimal(x >> power));
        Assert.Equal((x&y)<< power, (x<< power) &(y<< power));
    }


    [Theory]
    [ClassData(typeof(IntegerByteData))]
    public void Mod(byte x, byte y)
    {
        const int power = 2;
        Assert.Equal(x% (2 << power - 1), x&((2 << power - 1) - 1));
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

    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 2)]
    [InlineData(8, 3)]
    [InlineData(16, 4)]
    public void Ruler3(int x, int y)
    {
        Assert.Equal(y, x.GetRuler3());
    }

    //[Theory]
    //[InlineData(2, 1)]
    //[InlineData(4, 2)]
    //[InlineData(8, 3)]
    //[InlineData(16, 4)]
    //public void Ruler2(int x, int k)
    //{
    //    Assert.Equal(k, x.GetRuler2());
    //}

    [Theory]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(5, true)]
    [InlineData(7, true)]
    [InlineData(9, false)]
    [InlineData(11, true)]
    [InlineData(13, true)]
    [InlineData(15, false)]
    public void IsPrimeBigEndian(int x, bool isPrime)
    {
        Assert.Equal(isPrime, x.IsPrimeBigEndian());
    }

    [Theory]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(5, true)]
    [InlineData(7, true)]
    [InlineData(9, false)]
    [InlineData(11, true)]
    [InlineData(13, true)]
    [InlineData(15, false)]
    public void IsPrimeLittleEndian(int x, bool isPrime)
    {
        Assert.Equal(isPrime, x.IsPrimeLittleEndian());
    }

    [Theory]
    [InlineData(32, 5)]

    public void LeftmostBitPlace(int x, int leftmostBitPlace)
    {
        Assert.NotEqual(leftmostBitPlace, x.LeftMostBitPlace1());
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(3, 2)]
    [InlineData(4, 4)]
    [InlineData(5, 4)]
    [InlineData(8, 8)]
    [InlineData(9, 8)]
    [InlineData(16, 16)]
    [InlineData(32, 32)]
    [InlineData(64, 64)]
    [InlineData(128, 128)]
    public void ExtractLeftMostBit(int x, int ExtractLeftMostBit)
    {
        Assert.Equal(ExtractLeftMostBit, x.ExtractLeftMostBit());
        Assert.Equal(ExtractLeftMostBit, x.ExtractLeftMostBit2());
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    public void SideWaySums(int x)
    {
        Assert.Equal(x.GetRuler3(), 1 + (x-1).SidewaysSum() - x.SidewaysSum());
    }
    
    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    public void SideWaySums2(int n)
    {
        Assert.Equal(n - n.SidewaysSum(), Enumerable.Range(1, n).Select(i => i.GetRuler()).Sum());
    }
}
