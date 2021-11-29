using Microsoft.Quantum.Simulation.Simulators;
using MyFSharpInterop.Number;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Buffers;
using System.Threading.Tasks;
using CSharpClassLibrary.Native;
using CSharpClassLibrary.Reflection;

namespace CSharpConsoleApp;

static class Program
{
    static async Task Main(string[] args)
    {
        Reflection.Test();

        //Console.WriteLine($"Method from native class: {Test(2)}");

        //int productOfTwo = Number.productOfTwo(1, 2);
        //Console.WriteLine(string.Format("{0}", productOfTwo));
        //int sumOfThree = Number.sumOfThree(1, 2, 3);
        //Console.WriteLine(string.Format("{0}", sumOfThree));

        //var bits = new[] { false, true, false };
        //using var sim = new QuantumSimulator();
        //Console.WriteLine($"Input: {bits.ToDelimitedString()}");

        //var restored = await RunAlgorithm.Run(sim, new QArray<bool>(bits));
        //Console.WriteLine($"Output: {restored.ToDelimitedString()}");
        //Assert(bits.Parity() == restored.Parity());

        //PeekableEnumerableAdapter<char> it1 = new PeekableEnumerableAdapter<char>("if abc");
        //PeekableEnumerableAdapter<char> it2 = new PeekableEnumerableAdapter<char>("true abc");
        //Token token1 = Token.ParsingUsingIterator(it1);

        //Memory<char> memory2 = new char[64];
        //IMemoryOwner<char> owner = MemoryPool<char>.Shared.Rent();
        //Console.Write("Enter a number: ");
        //try
        //{
        //    var value = Int32.Parse(Console.ReadLine());
        //    var memory = owner.Memory;
        //    Memory.WriteInt32ToBuffer(value, memory);
        //    Memory.DisplayBufferToConsole(owner.Memory.Slice(0, value.ToString().Length));
        //}
        //catch (FormatException)
        //{
        //    Console.WriteLine("You did not enter a valid number.");
        //}
        //catch (OverflowException)
        //{
        //    Console.WriteLine($"You entered a number less than {Int32.MinValue:N0} or greater than {Int32.MaxValue:N0}.");
        //}
        //finally
        //{
        //    owner?.Dispose();
        //}

        //using (var owner2 = MemoryPool<char>.Shared.Rent())
        //{
        //    var memory = owner2.Memory;
        //    var span2 = memory.Span;
        //    while (true)
        //    {
        //        int value = int.Parse(Console.ReadLine());
        //        if (value < 0)
        //            return;

        //        int numCharsWritten = ToBuffer(value, span2);
        //        Log(memory.Slice(0, numCharsWritten));
        //    }
        //}

    }

    static string ToDelimitedString(this IEnumerable<bool> values) =>
        string.Join(", ", values.Select(x => x.ToString()));
}
