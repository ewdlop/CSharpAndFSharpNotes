using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpClassLibrary.Misc
{
    public class OddValueExtractor
    {
        public OddValueExtractor(ReadOnlyMemory<int> input)
        {

        }
        public bool TryReadNextOddValue(out int value){
            value = 1;
            return true;
        }

        void PrintAllOddValues(ReadOnlyMemory<int> input)
        {
            var extractor = new OddValueExtractor(input);
            while (extractor.TryReadNextOddValue(out int value))
            {
                Console.WriteLine(value);
            }
        }
    }
}
