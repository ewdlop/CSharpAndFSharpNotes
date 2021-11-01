using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using MyFSharpInterop.Number;
using MyQSharpInterop.Operations;
using static System.Diagnostics.Debug;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System;
using System.Buffers;
using System.Threading.Tasks;
using System.IO;
using CSharpClassLibrary.Misc;

namespace CSharpConsoleApp
{
    static class Program
    {
        [DllImport("NativeClassLibrary.dll")]
        public static extern int Test(int value);
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Method from native class: {Test(2)}");

            int productOfTwo = Number.productOfTwo(1, 2);
            Console.WriteLine(string.Format("{0}", productOfTwo));
            int sumOfThree = Number.sumOfThree(1, 2, 3);
            Console.WriteLine(string.Format("{0}", sumOfThree));

            var bits = new[] { false, true, false };
            using var sim = new QuantumSimulator();
            Console.WriteLine($"Input: {bits.ToDelimitedString()}");

            var restored = await RunAlgorithm.Run(sim, new QArray<bool>(bits));
            Console.WriteLine($"Output: {restored.ToDelimitedString()}");
            Assert(bits.Parity() == restored.Parity());

            //PeekableEnumerableAdapter<char> it1 = new PeekableEnumerableAdapter<char>("if abc");
            //PeekableEnumerableAdapter<char> it2 = new PeekableEnumerableAdapter<char>("true abc");
            //Token token1 = Token.ParsingUsingIterator(it1);

            Memory<char> memory2 = new char[64];
            IMemoryOwner<char> owner = MemoryPool<char>.Shared.Rent();
            Console.Write("Enter a number: ");
            try
            {
                var value = Int32.Parse(Console.ReadLine());
                var memory = owner.Memory;
                WriteInt32ToBuffer(value, memory);
                DisplayBufferToConsole(owner.Memory.Slice(0, value.ToString().Length));
            }
            catch (FormatException)
            {
                Console.WriteLine("You did not enter a valid number.");
            }
            catch (OverflowException)
            {
                Console.WriteLine($"You entered a number less than {Int32.MinValue:N0} or greater than {Int32.MaxValue:N0}.");
            }
            finally
            {
                owner?.Dispose();
            }

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

        static bool Parity(this IEnumerable<bool> bitVector) =>
            bitVector.Aggregate(
                (acc, next) => acc ^ next
            );

        static string ToDelimitedString(this IEnumerable<bool> values) =>
            string.Join(", ", values.Select(x => x.ToString()));

        static void WriteInt32ToBuffer(int value, Memory<char> buffer)
        {
            var strValue = value.ToString();

            var span = buffer.Span;
            for (int ctr = 0; ctr < strValue.Length; ctr++)
                span[ctr] = strValue[ctr];
        }

        static void WriteInt32ToBuffer2(int value, Memory<char> buffer)
        {
            var strValue = value.ToString();

            var span = buffer.Slice(0, strValue.Length).Span;
            strValue.AsSpan().CopyTo(span);
        }


        static void DisplayBufferToConsole(ReadOnlyMemory<char> buffer) =>
            Console.WriteLine($"Contents of the buffer: '{buffer}'");

        private static int ToBuffer(int value, Span<char> span)
        {
            string strValue = value.ToString();
            int length = strValue.Length;
            strValue.AsSpan().CopyTo(span.Slice(0, length));
            return length;
        }
        static Task Log(ReadOnlyMemory<char> message)
        {
            // Run in the background so that we don't block the main thread while performing IO.
            return Task.Run(() =>
            {
                StreamWriter sw = File.AppendText(@".\input-numbers.dat");
                sw.WriteLine(message);
                sw.Flush();
            });
        }

        static void Log2(ReadOnlyMemory<char> message)
        {
            string defensiveCopy = message.ToString();
            // Run in the background so that we don't block the main thread while performing IO.
            Task.Run(() =>
            {
                StreamWriter sw = File.AppendText(@".\input-numbers.dat");
                sw.WriteLine(defensiveCopy);
                sw.Flush();
            });
        }

        static void Log3(ReadOnlyMemory<char> message)
        {
            // Run in the background so that we don't block the main thread while performing IO.
            Task.Run(() =>
            {
                string defensiveCopy = message.ToString();
                StreamWriter sw = File.AppendText(@".\input-numbers.dat");
                sw.WriteLine(defensiveCopy);
                sw.Flush();
            });
        }

        static void PrintAllOddValues(ReadOnlyMemory<int> input)
        {
            var extractor = new OddValueExtractor(input);
            while (extractor.TryReadNextOddValue(out int value))
            {
                Console.WriteLine(value);
            }
        }
    }
}