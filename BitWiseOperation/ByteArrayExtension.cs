using System.Linq;

namespace BitWiseOperation;

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
