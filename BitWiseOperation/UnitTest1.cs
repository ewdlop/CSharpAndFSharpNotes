using System;
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

    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 2)]
    [InlineData(8, 3)]
    [InlineData(16, 4)]
    public void Ruler2(int x, int k)
    {
        Assert.Equal(k, x.GetRuler2());
        Assert.Equal(x.GetRuler2() + 1, (2 * x).GetRuler2());
        Assert.Equal((x ^ k).GetRuler(), (x - k).GetRuler2());
    }
}
