using Nito;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CSharpClassLibrary.DataFlow;

public static partial class BlockDataFlow
{
    public static void Run()
    {
        TransformBlock<string, (string, byte[])> fetchImageFlag = new(
            async urlImage => {
                using HttpClient httpClient = new HttpClient();
                byte[] data = await httpClient.GetByteArrayAsync(urlImage);
                return (urlImage, data);
            }
        );
        List<string> urlFlags = new List<string>
        {
            "Italy#/media/File:Flag_of_Italy.svg",
            "Spain#/media/File:Flag_of_Spain.svg",
            "United_States#/media/File:Flag_of_the_United_States.svg"
        };
        foreach (string urlFlag in urlFlags)
        {
            fetchImageFlag.Post($"https://en.wikipedia.org/wiki/{urlFlag}");
        }
        ActionBlock<(string, byte[])> saveData = new ActionBlock<(string, byte[])>(async data => {
            (string urlImage, byte[] image) = data;
            string filePath = urlImage[(urlImage.IndexOf("File:") + 5)..];
            await File.WriteAllBytesAsync(filePath, image);
        });
        fetchImageFlag.LinkTo(saveData);
    }
}
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
