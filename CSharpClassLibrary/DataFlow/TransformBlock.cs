using Nito;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CSharpClassLibrary.DataFlow;

public static class TransformBlockExtension
{
    public static TransformBlock<Try<TInput>, Try<TOutput>> RailwayTransform<TInput, TOutput>(this Func<TInput, TOutput> func)
    {
        return new TransformBlock<Try<TInput>, Try<TOutput>>(t => t.Map(func));
    }

    private static async Task Test()
    {
        var subtractBlock = RailwayTransform<int, int>(value => value - 2);
        var divideBlock = RailwayTransform<int, int>(value => 60 / value);
        var multiplyBlock = RailwayTransform<int, int>(value => value * 2);
        var options = new DataflowLinkOptions { 
            PropagateCompletion = true
        };
        
        subtractBlock.LinkTo(divideBlock, options);
        divideBlock.LinkTo(multiplyBlock, options);
        
        // Insert data items into the first block.
        subtractBlock.Post(Try.FromValue(5));
        subtractBlock.Post(Try.FromValue(2));
        subtractBlock.Post(Try.FromValue(4));
        while (await multiplyBlock.OutputAvailableAsync())
        {
            Try<int> item = await multiplyBlock.ReceiveAsync();
            if (item.IsValue)
                Console.WriteLine(item.Value);
            else
                Console.WriteLine(item.Exception.Message);
        }

        subtractBlock.Complete();
    }
}
