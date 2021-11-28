using System;

namespace CSharpClassLibrary.Struct
{
    public readonly ref struct ConversionRequest
    {
        public ConversionRequest(double rate, ReadOnlySpan<double> values)
        {
            Rate = rate;
            Values = values;
        }

        public double Rate { get; }
        public ReadOnlySpan<double> Values { get; }
    }
}


