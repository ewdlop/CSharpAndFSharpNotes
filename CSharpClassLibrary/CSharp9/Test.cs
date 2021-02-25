using System;
using System.Collections.Generic;
using CSharpClassLibrary.CSharp9.Record;

namespace CSharpClassLibrary.CSharp9
{
    public class Test1
    {
        public int X { get; set; }
        public Test1(int x, int y)
        {
            X = x;
        }
    }

    static class Test
    {
        public static IEnumerator<T> GetEnumerator<T>(this IEnumerator<T> enumerator) => enumerator;
        public static T? MyFirstOrDefault<T>(this IEnumerable<T> collection)
        {
            return default;
        }

        public static void Run()
        {
            //Target-Typed Conditional Expression
            var test = new Test1(true? 1 : 2, 3);

            Console.WriteLine("Hello World!");
            var person = new Person("Jack", "Palmer");
            //Deconstructing tuples
            var (first, last) = person;
            Console.WriteLine(first);
            Console.WriteLine(last);
            Person clone = person with { };
            Console.WriteLine(clone);
            Console.WriteLine(clone);

            Location town = new() { Name = "HomeTown" };
            List<Location> list = new();

            var north = new Direction(0.0f, 1.0f);
            var south = north with { Y = -1.0f };

            //Person brother = person with { FirstName = "Paul" };

            //Static anonymous functions
            const string text = "{0} is a beautiful country !"; // text must be declared as const
            FunctioanlPrint(static country => string.Format(text, country)); // text is not captured
        }


        private static void FunctioanlPrint(Func<string, string> func)
        {
            foreach (var country in (IEnumerator<string>)new List<string> { "France", "Canada", "Japan", "USA"})
                Console.WriteLine(func(country));
        }
    }
}