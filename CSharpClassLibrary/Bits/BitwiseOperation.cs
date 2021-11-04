using System.Collections.Generic;
using System.Linq;

namespace CSharpClassLibrary.Bits
{
    public static class BitwiseOperation
    {
        private static bool Parity(this IEnumerable<bool> bitVector) =>
            bitVector.Aggregate(
                (acc, next) => acc ^ next
            );
    }
}