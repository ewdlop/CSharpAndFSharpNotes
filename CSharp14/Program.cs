using CSharp14;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;
Console.WriteLine("C# 14 Extension Methods with 'extension' keyword");
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
Console.WriteLine("Numbers less than 5: " + string.Join(", ", numbers.小於(5)));
Console.WriteLine("Numbers equal to 5: " + string.Join(", ", numbers.等於(5)));
Console.WriteLine("Numbers greater than 5: " + string.Join(", ", numbers.大於(5)));
string palindrome = "racecar";
Console.WriteLine($"Is '{palindrome}' a palindrome? {palindrome.是迴文()}");
//https://devblogs.microsoft.com/dotnet/csharp-exploring-extension-members/
//https://visualstudio.microsoft.com/insiders/

Console.WriteLine($"Reversed '{palindrome}': {palindrome.反轉()}");
int primeCandidate = 29;
Console.WriteLine($"Is {primeCandidate} a prime number? {primeCandidate.是質數()}");
int factorialNumber = 5;
Console.WriteLine($"{factorialNumber}! = {factorialNumber.階乘()}");
int divisorNumber = 28;
Console.WriteLine($"Factors of {divisorNumber}: " + string.Join(", ", divisorNumber.因數()));

Test test = new Test(42);
test?.X = 100;

namespace CSharp14
{
    //A header
    public delegate bool TryParse<T>(string text, out T result);


    public class Test(int x)
    {
        public int? X = x;
    }


    public class TryParse
    {
        public static TryParse<int> parse1 = Int32.TryParse;
        public static TryParse<int> parse2 = (string text, out int result) => parse1(text, out result);
    }

    public static class IEnumerableExtensions
    {
        extension(IEnumerable<int> source)
        {
            public IEnumerable<int> 小於(int threshold)
                => source.Where(x => x < threshold);
        }

        extension(IEnumerable<int> source)
        {
            public IEnumerable<int> 等於(int threshold)
                => source.Where(x => x > threshold);
        }

        extension(IEnumerable<int> source)
        {
            public IEnumerable<int> 大於(int threshold)
                => source.Where(x => x > threshold);
        }

        extension(string s)
        {
            public string 反轉()
                => new string(s.Reverse().ToArray());
        }  

        extension(string s)
        {
            public bool 是迴文()
                => s.SequenceEqual(s.Reverse());
        }

        extension(int number)
        {
            public bool 是質數()
            {
                if (number <= 1) return false;
                for (int i = 2; i <= Math.Sqrt(number); i++)
                {
                    if (number % i == 0) return false;
                }
                return true;
            }
        }

        extension(int number)
        {
            public int 階乘()
            {
                if (number < 0) throw new ArgumentException("Number must be non-negative");
                return number == 0 ? 1 : number * (number - 1).階乘();
            }
        }

        extension(int number)
        {
            public IEnumerable<int> 因數()
            {
                for (int i = 1; i <= number; i++)
                {
                    if (number % i == 0) yield return i;
                }
            }
        }


        extension(IEnumerable<int> source)
        {
            public IEnumerable<int> As大於(int threshold)
                => source.Where(x => x > threshold);
        }
#if false
        extension(IEnumerable<T> source) where T : INumber<T>
        {
            public IEnumerable<T> As大於(T threshold)
                => source.Where(x => x > threshold);
        }
#endif

        extension<T>(IEnumerable<T> source) where T : INumber<T>
        {
            public IEnumerable<T> As大於(T threshold)
                => source.Where(x => x > threshold);
        }
    }


    public interface I
    {
        
    }

    public interface II
    {
    }

    public interface III
    {
    }

    public static class INumberExtensions
    {

#if false
            // algebra on interfaces
            public static III Combine(this I one, II two) 
            =>  I + II;
#endif

        public static T Minus<T>(this T number,T other) where T : INumber<T>
            => number - other;

        extension<T>(T number) where T : INumber<T>
        {
            public IEnumerable<T> As因數()
            {
                for (T i = T.One; i <= number; i++)
                {
                    if (number % i == T.Zero) yield return i;
                }
            }

            //public IEnumerable<T?> 大於(T threshold)
            //{
            //    yield return number > threshold ? number : default;
            //}

            public IEnumerable<T> As大於(T threshold)
            {
                if (number > threshold) yield return number;
            }
            public INumber<T> Add(T other)
                => number + other;
        }


    }
}
