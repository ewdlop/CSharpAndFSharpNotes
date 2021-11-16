
// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

Console.WriteLine("Hello, World!");

string filename = $"{AssemblyDirectory}/DontRunMe.exe";
{
    try
    {
        foreach (var halted in FourtyTwo(filename, args))
        {
            if(halted)
            {
                break;
            }
            else
            {
                return;
            }
        }
    }
    catch (Exception)
    {
        for (; ; );
    }
    for (; ; );
}


IEnumerable<bool> FourtyTwo(string filename, string[] args)
{
    yield return true;
    //try
    //{
    //    var process = Process.Start(filename, args);
    //    while (!process.HasExited)
    //    {
    //        yield return false;
    //    }
    //    yield return true;
    //}
    //catch(Win32Exception)
    //{
    //    yield return true;
    //}
}

string? AssemblyDirectory()
{
    string codeBase = Assembly.GetExecutingAssembly().Location;
    UriBuilder uri = new UriBuilder(codeBase);
    string path = Uri.UnescapeDataString(uri.Path);
    return Path.GetDirectoryName(path);
}