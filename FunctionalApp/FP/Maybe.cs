using System;

namespace CSharpClassLibrary.FP
{
    public class Mabye<T>
    {
        public T? Value { get; private set; }
        public Mabye(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
        }
        public Mabye() { }
        public Mabye<U> Bind<U>(Func<T, U> func) =>
            Value is null ? Mabye<U>.None() : new Mabye<U>(func(Value));
        public static Mabye<T> None() => new();
    }

    public static partial class GenericMabyeExtension
    {
        public static Mabye<T> Unit<T>(this T value) => new(value);
    }
}
