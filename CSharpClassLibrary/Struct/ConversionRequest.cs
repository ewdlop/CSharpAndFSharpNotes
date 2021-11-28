using System;
namespace CSharpClassLibrary.Struct
{
    public readonly ref struct ConversionRequest
    {
        private struct X { }
        public ConversionRequest(double rate, ReadOnlySpan<double> values)
        {
            Rate = rate;
            Values = values;
        }

        public double Rate { get; }
        public ReadOnlySpan<double> Values { get; }
    }
}


