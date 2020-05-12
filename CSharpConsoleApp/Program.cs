using System;
using MyFSharpInterop.Number;

namespace CSharpConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int productOfTwo = Number.productOfTwo(1, 2);
            Console.WriteLine(string.Format("{0}", productOfTwo));
            int sumOfThree = Number.sumOfThree(1, 2, 3);
            Console.WriteLine(string.Format("{0}", sumOfThree));
        }
    }
}
