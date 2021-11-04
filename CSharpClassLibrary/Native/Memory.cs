using System;
using System.IO;
using System.Threading.Tasks;

namespace CSharpClassLibrary.Native
{
    public class Memory
    {
        public static void WriteInt32ToBuffer(int value, Memory<char> buffer)
        {
            var strValue = value.ToString();

            var span = buffer.Span;
            for (int ctr = 0; ctr < strValue.Length; ctr++)
                span[ctr] = strValue[ctr];
        }

        public static void WriteInt32ToBuffer2(int value, Memory<char> buffer)
        {
            var strValue = value.ToString();

            var span = buffer.Slice(0, strValue.Length).Span;
            strValue.AsSpan().CopyTo(span);
        }


        public static void DisplayBufferToConsole(ReadOnlyMemory<char> buffer) =>
            Console.WriteLine($"Contents of the buffer: '{buffer}'");

        private static int ToBuffer(int value, Span<char> span)
        {
            string strValue = value.ToString();
            int length = strValue.Length;
            strValue.AsSpan().CopyTo(span.Slice(0, length));
            return length;
        }
        public static Task Log(ReadOnlyMemory<char> message)
        {
            // Run in the background so that we don't block the main thread while performing IO.
            return Task.Run(() =>
            {
                StreamWriter sw = File.AppendText(@".\input-numbers.dat");
                sw.WriteLine(message);
                sw.Flush();
            });
        }

        public static void Log2(ReadOnlyMemory<char> message)
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

        public static void Log3(ReadOnlyMemory<char> message)
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

        public static void PrintAllOddValues(ReadOnlyMemory<int> input)
        {
            //var extractor = new OddValueExtractor(input);
            //while (extractor.TryReadNextOddValue(out int value))
            //{
            //    Console.WriteLine(value);
            //}
        }
    }
}
