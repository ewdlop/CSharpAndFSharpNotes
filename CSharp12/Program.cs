using CSharp12;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Point = (double x, double y);

Point point = (3.14, 2.71);
Rectangle rectangle = new Rectangle(point);

var sum = (int x=0, int y=0) => x+y;
sum(1,2);
sum();

var counter = (params int[] xs) => xs.Length;
counter(); // 0
counter(1, 2, 3); // 3

Buffer10<int> GetBuffer() => default;

bool append = true;


List<int> l = [1, 2];
List<int> l2 = [1, 2, .. l];
List<int> l3 = [1 ,2 ,.. append ? l2 : [], 3, 4];

NameOf<int>.StringLength("Hello");

public class NameOf<T> where T : INumber<T>
{
    public static string S => string.Empty;
    public static T StaticField = T.Zero;
    public string NameOfLength => nameof(NameOf<T>.S.Length);
    public static void NameOfExamples()
    {
        Console.WriteLine(nameof(NameOf<T>.S.Length));
        Console.WriteLine(nameof(StaticField.One));
    }
    [Description($"String {nameof(NameOf<T>.S.Length)}")]
    public static int StringLength(string s) => s.Length;
}

namespace CSharp12
{
    public class Rectangle(Point p)
    {
        public double X => p.x;
        public double Y => p.y;
        public double Area { get; init; } = p.x * p.y;
        public Rectangle(double x, double y) : this(new Point(x, y)) { }
    }
    public class Square(Point p) : Rectangle(p)
    {
        public Square(double x) : this(new Point(x, x)) { }
    }

    [System.Runtime.CompilerServices.InlineArray(10)]
    public struct Buffer10<T>
    {
        private T _element0;

        public T this[int i]
        {
            get => this[i];
            set => this[i] = value;
        }
    }

    public static class MyInterceptor
    {
        //[InterceptsLocation("Program.cs", line: 39, character: 13)]
        public static void StringLength<T>(this NameOf<T> t, string s) where T : INumber<T>
        {
            Console.WriteLine($"Intercepted {s}");
        }
    }
}
