using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using FASTER.core;
using System.Buffers;
using System.Diagnostics;
//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

//CustomTaskScheduler customTaskScheduler = new();
//CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
//TaskCreationOptions creationOptions =
//    TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning | TaskCreationOptions.AttachedToParent
//   | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler | TaskCreationOptions.RunContinuationsAsynchronously;

//Task.Factory.StartNew(async () =>
//{
//    await Task.Delay(5000, cancellationTokenSource.Token);
//    Console.WriteLine("Hello, World!");

//}, cancellationTokenSource.Token, creationOptions, customTaskScheduler);


////https://devblogs.microsoft.com/oldnewthing/20120120-00/?p=8493
////https://devblogs.microsoft.com/oldnewthing/20200819-00/?p=104093
////https://stackoverflow.com/questions/3033771/file-i-o-with-streams-best-memory-buffer-size
//await using FileStream appendStream = new FileStream("sad", new FileStreamOptions()
//{
//    Access = FileAccess.Write | FileAccess.Read | FileAccess.ReadWrite,
//    Mode = FileMode.Append | FileMode.CreateNew | FileMode.Open | FileMode.OpenOrCreate | FileMode.Truncate | FileMode.Append,
//    Options = FileOptions.Asynchronous | FileOptions.WriteThrough | FileOptions.DeleteOnClose | FileOptions.SequentialScan,
//    BufferSize = 4096,
//    Share = FileShare.None | FileShare.Read | FileShare.Write | FileShare.ReadWrite | FileShare.Delete | FileShare.Inheritable,
//    PreallocationSize = 0
//});

//public class CustomTaskScheduler : TaskScheduler
//{

//    protected override IEnumerable<Task>? GetScheduledTasks()
//    {
//        throw new NotImplementedException();
//    }

//    protected override void QueueTask(Task task)
//    {
//        throw new NotImplementedException();
//    }

//    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
//    {
//        throw new NotImplementedException();
//    }
//}
BenchmarkRunner.Run<BenchmarkTest>();

int[] buffer2 = ArrayPool<int>.Shared.Rent(5);

try
{
    // Slice the span, as it might be larger than the requested size
    Span<int> span2 = buffer2.AsSpan(0, 5);

    // Use the span here
}
finally
{
    ArrayPool<int>.Shared.Return(buffer2);
}

using SpanOwner<int> buffer = SpanOwner<int>.Allocate(5);

Span<int> span = buffer.Span;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net60)]
public class BenchmarkTest
{

    [Benchmark]
    public void Test()
    {
        GetBytesFromFile("test.txt");
    }

    [Benchmark]
    public void Test2()
    {
        GetBytesFromFile2("test.txt");
    }        

    static MemoryOwner<byte> GetBytesFromFile(string path)
    {
        using Stream stream = File.OpenRead(path);

        MemoryOwner<byte> buffer = MemoryOwner<byte>.Allocate((int)stream.Length);

        stream.Read(buffer.Span);
        return buffer;
    }


    static (byte[] Buffer, int Length) GetBytesFromFile2(string path)
    {
        using Stream stream = File.OpenRead(path);

        byte[] buffer = ArrayPool<byte>.Shared.Rent((int)stream.Length);
        
        stream.Read(buffer, 0, (int)stream.Length);
        return (buffer, (int)stream.Length);
    }

    static async Task<(byte[] Buffer, int Length)> GetBytesFromFile2Async(string path)
    {
        using Stream stream = File.OpenRead(path);

        byte[] buffer = ArrayPool<byte>.Shared.Rent((int)stream.Length);

        await stream.ReadAsync(buffer.AsMemory(0, (int)stream.Length));

        return (buffer, (int)stream.Length);
    }

    static async Task<MemoryOwner<byte>> GetBytesFromFileAsync(string path)
    {
        using Stream stream = File.OpenRead(path);

        MemoryOwner<byte> buffer = MemoryOwner<byte>.Allocate((int)stream.Length);

        await stream.ReadAsync(buffer.Memory);

        return buffer;
    }

    static string GetHost(string url)
    {
        // We assume the input might start either with eg. https:// (or other prefix),
        // or directly with the host name. Furthermore, we also assume that the input
        // URL will always have a '/' character right after the host name.
        // For instance: "https://docs.microsoft.com/dotnet/api/system.string.intern".
        int
            prefixOffset = url.AsSpan().IndexOf(stackalloc char[] { ':', '/', '/' }),
            startIndex = prefixOffset == -1 ? 0 : prefixOffset + 3,
            endIndex = url.AsSpan(startIndex).IndexOf('/');

        // In this example, it would be "docs.microsoft.com"
        ReadOnlySpan<char> span = url.AsSpan(startIndex, endIndex);

        return StringPool.Shared.GetOrAdd(span);
    }
    static void Faster()
    {
        FasterKVSettings<long, long> settings = new FasterKVSettings<long, long>("c:/temp"); // backing storage device
        using FasterKV<long, long> store = new FasterKV<long, long>(settings);

        // Create a session per sequence of interactions with FASTER
        // We use default callback functions with a custom merger: RMW merges input by adding it to value
        using ClientSession<long, long, long, long, Empty, IFunctions<long, long, long, long, Empty>>
            session = store.NewSession(new SimpleFunctions<long, long>((a, b) => a + b));
        


        long key = 1, value = 1, input = 10, output = 0;

        // Upsert and Read
        session.Upsert(ref key, ref value);
        session.Read(ref key, ref output);
        Debug.Assert(output == value);

        //https://en.wikipedia.org/wiki/Read%E2%80%93modify%E2%80%93write
        // Read-Modify-Write (add input to value)
        session.RMW(ref key, ref input);
        session.RMW(ref key, ref input, ref output);
        Debug.Assert(output == value + 20);
    }

    public static string FindCollectionName(Type type) =>
    type switch
    {
        _ when typeof(int).IsAssignableFrom(type) => "Messages",
        _ when typeof(double).IsAssignableFrom(type) => "UserData",
        _ => ""
    };
}

public static class ListPattern
{
    public static void Test()
    {
        int[] fibonacci = { 1, 2, 3, 5, 8 };
        bool result = false;

        // result is false, length not matching
        result = fibonacci is [_, _, 3, _];

        // result is false, 3 is not at same position
        result = fibonacci is [_, _, _, 3, _];

        // result is false, length is matching but 2 and 3 not at same positions
        result = fibonacci is [2, _, 3, _, _];

        // result is true, single element and its position and length is matching
        result = fibonacci is [1, _, _, _, _];

        // result is true, multiple elements and their positions and length is matching
        result = fibonacci is [1, _, 3, _, _];

        result = fibonacci switch
        {
            [_, _, 3, _] => true,
            [_, _, _, 3, _] => true,
            [2, _, 3, _, _] => true,
            [1, _, _, _, _] => true,
            [0, _, 3, _, _] => true,
            _ => false
        };

        var s = fibonacci.AsSpan() switch
        {
            [] => 0,
            /*[var first, _] => first*/
            [var head, var next,..var rest] => head + next + rest[0],
            ////[var first, .., var last] => first + last,
            ////[var first, ..var anyBesideLast, var last] => first + anyBesideLast[0] + last,
            [var head, .. var tail] => head + tail[0],
        };

        List<int> list = new List<int> { 1, 2, 3, 5, 8 };
        int s2 = list.AsSpan() switch
        {
            [var head, var next, .. var rest] => head + next + rest[0],
            _ => 0
        };
    }
}

