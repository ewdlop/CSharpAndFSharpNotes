using System;
using System.Threading.Tasks;

namespace CSharpClassLibrary.FP
{
    public class Future<T>
    {
        public Task<T> Value { get; private set; }

        public Future(T value) => Value = Task.FromResult(value);

        public Future(Task<T> task) => Value = task;

        public Future<U> Bind<U>(Func<T, Future<U>> func) => 
            new(Value.ContinueWith(t => func(t.Result).Value).Unwrap());

        public void OnComplete(Action<T> action)
        {
            Value.ContinueWith(t => action(t.Result));
        }
    }

    public static partial class GenericFutureExtension
    {
        public static Future<T> Unit<T>(this T value) => new(value);
    }

}
