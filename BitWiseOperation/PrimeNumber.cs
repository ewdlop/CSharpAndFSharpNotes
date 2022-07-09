using System;

namespace BitWiseOperation;

public static class PrimeNumber
{
    public static readonly ulong[] P = new ulong[8] {
        8562236959291389569, 
        5490541475240438148,
        10604886793545748773,
        2488346084741551186, 
        865326974818135056, 
        15159296949658734888, 
        11547805820870525648,
        364943612004343974
    };

    public static readonly ulong[] Q = new ulong[8]
    {
        9326130857677998958,
        2420264844357422130,
        11856114285606614217,
        5343664792731521348,
        591265818948550704,
        1480075160427234891,
        810845881194529285,
        7279093702936561824
    };
    //rever byte order
    //public static readonly ulong[] Q = P.Select(value =>
    //     (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
    //        (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
    //     (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
    //        (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56).ToArray();


    public static bool IsPrimeBigEndian(this int x)
    {
        if (x switch {
                <=0 => false,
                2 => true,
                >= 1024 => throw new NotSupportedException("Does not work for integer greater than or equal to 1024"),
                _ => true
            })
        {
            if(x == 2) return true;
            int k = (x - 1) / 2;
            ulong p = P[k / 64]<<(k & 63);
            return p >> 63 == 1;
        }
        else
        {
            return false;
        }
    }
    public static bool IsPrimeLittleEndian(this int x)
    {
        if (x switch
            {
                <= 0 => false,
                2 => true,
                >= 1024 => throw new NotSupportedException("Does not work for integer greater than or equal to 1024"),
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
