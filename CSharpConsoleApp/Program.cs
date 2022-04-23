using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpConsoleApp;

internal static class Program
{
    public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> tasks)
    {
        return await Task.WhenAll(tasks);
    }

    static async Task Main(string[] args)
    {
        string path = "C:\\Users\\Ray\\Desktop\\Arcady Block Key";
        var group = (await Directory.GetFiles(path, "*.pdf", SearchOption.AllDirectories)
            .Select(async (filePath, index) => new
            {
                key = new FileInfo(filePath).DirectoryName?.Replace(path, string.Empty),
                value = await File.ReadAllBytesAsync(filePath),
            }).WhenAll()).GroupBy(x => x.key).ToLookup(x => x.Key, x => x.ToList()); ;

        //var x = group
        //foreach(var y in x)
        //{
        //    y.SelectMany(x=>x).ToList().ForEach(x=>Console.WriteLine(x));
        //}
        if(group.Contains(""))
        {
            Console.WriteLine("1");
        }

        Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList().ForEach(t => Console.WriteLine(t));
    }

    static string ToDelimitedString(this IEnumerable<bool> values) =>
        string.Join(", ", values.Select(x => x.ToString()));
}
