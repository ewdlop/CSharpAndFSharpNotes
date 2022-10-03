using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTestProject;

public class Z
{
    public int x { get; set; }
    public int y { get; set; }

    //public override bool Equals(object other)
    //{
    //    return Equals(other as Z);
    //}

    //public virtual bool Equals(Z other)
    //{
    //    if (other == null) { return false; }
    //    if (ReferenceEquals(this, other)) { return true; }
    //    return x == other.x && y == other.y;
    //}
}
public record X(int x, int y, Z z);
public record X2(int x, int y, X z);
public record X3(int x, int y);
public struct Y
{
    public int x { get; set; }
    public int y { get; set; }
}

public struct Y2
{
    public int x { get; set; }
    public int y { get; set; }
    public Z z { get; set; }
    //public X x1 { get; set; }
}

public struct Y3
{
    public int x { get; set; }
    public int y { get; set; }
    public X3 x1 { get; set; }
}

public struct Y4
{
    public int x { get; set; }
    public int y { get; set; }
    public Y5 y1 { get; set; }
}

public struct Y5
{
    public int x { get; set; }
    public int y { get; set; }
}

public class UnitTest1
{
    [Fact]
    public void TestClass()
    {
        var z1 = new Z() { x = 1, y = 2 };
        var z2 = new Z() { x = 1, y = 2 };
        Assert.False(z1 == z2);
        Assert.False(z1.Equals(z2));
    }

    [Fact]
    public void TestRecord()
    {
        var x = new X(1, 2, new Z() { x = 3, y = 4 });
        var y = new X(1, 2, new Z() { x = 3, y = 4 });
        Assert.False(x == y);
        Assert.False(x.Equals(y));
        var x1 = new X(1, 2, null);
        var y1 = new X(1, 2, null);
        Assert.True(x1 == y1);
        Assert.True(x1.Equals(y1));
        var x2 = new X2(1, 2, x1);
        var y2 = new X2(1, 2, y1);
        Assert.True(x2 == y2);
        Assert.True(x2.Equals(y2));
        y = x;
        Assert.True(y == x);
        y = x with { };
        Assert.True(y == x);
    }

    [Fact]
    public void TestStruct()
    {
        var y1 = new Y() { x = 1, y = 2 };
        var y2 = new Y() { x = 1, y = 2 };
        var y3 = new Y() { x = 1, y = 3 };
        Assert.True(y1.Equals(y2));
        //Assert.True(y1 == y2);
        Assert.False(y1.Equals(y3));

        var y4 = new Y2() { x = 1, y = 2, z = new Z() { x = 3, y = 4 } };
        var y5 = new Y2() { x = 1, y = 2, z = new Z() { x = 3, y = 4 } };
        Assert.False(y4.Equals(y5));

        var y6 = new Y3() { x = 1, y = 2, x1 = new X3(3, 4) };
        var y7 = new Y3() { x = 1, y = 2, x1 = new X3(3, 4) };
        Assert.True(y6.Equals(y7));

        var y8 = new Y4() { x = 1, y = 2, y1 = new Y5() { x = 3, y = 4 } };
        var y9 = new Y4() { x = 1, y = 2, y1 = new Y5() { x = 3, y = 4 } };
        Assert.True(y8.Equals(y9));
    }

    [Fact]
    public void TestNull()
    {
        Assert.True(null == null);
        Assert.Null(null);
        X x1 = null;
        X x2 = null;
        Assert.True(x1 == x2);
        //Assert.True(x1.Equals(x2));
        Y? y1 = null;
        Y? y2 = null;
        //Assert.True(y1.Value.Equals(y2.Value));
        Assert.Equal(y1, y2);
    }

    [Fact]
    public void TestDeafult()
    {
        X x1 = default;
        X x2 = default;
        Assert.Null(x1);
        Assert.Null(x2);
        Assert.True(x1 == x2);

        Y? y1 = default;
        Y? y2 = default;
        Assert.Null(y1);
        Assert.Null(y2);

        Y2 y3 = default;
        Y2 y4 = default;
        Assert.True(y3.Equals(y4));
    }

    [Fact]
    public void Test()
    {
        var y = Enumerable.Range(1, 1000000).AsParallel().Aggregate(new List<int> { }, (accum, number) =>
        {
            accum.Add(number);
            return accum;
        });
        Assert.Equal(1000000, y.Count);
    }
}

public static class CustomCode
{
    public static int VersionCompare(string version1, string version2)
    {
        IEnumerable<int> version1Parts = version1.Split(".").Select(int.Parse);
        IEnumerable<int> version2Parts = version2.Split(".").Select(int.Parse); ;

        int version1PartsCount = version1Parts.Count();
        int version2PartsCount = version2Parts.Count();
        int maxPartsCount = Math.Max(version1PartsCount, version2PartsCount);

        List<int> comparingVersion1Parts = Enumerable.Repeat(0, maxPartsCount).ToList();
        List<int> comparingVersion2Parts = Enumerable.Repeat(0, maxPartsCount).ToList();
        
        comparingVersion1Parts.InsertRange(0, version1Parts);
        comparingVersion2Parts.InsertRange(0, version2Parts);
        for (int i = 0; i < maxPartsCount; i++)
        {
            if (comparingVersion1Parts[i] > comparingVersion2Parts[i])
            {
                return 1;
            }
            else if (comparingVersion1Parts[i] < comparingVersion2Parts[i])
            {
                return -1;
            }
        }
        return 0;
    }
}