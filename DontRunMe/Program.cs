
// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

Console.WriteLine("Hello, World!");

string filename = $"{AssemblyDirectory}/DontRunMe.exe";
{
    if (FourtyTwo(filename, args))
        for (; ; );
    else
        return;
}
catch (Win32Exception)
{
    for (; ; );
}

bool FourtyTwo(string filename, string[] args)
{
    var process = Process.Start(filename, args);
    process.WaitForExit();
    return true;
}

string? AssemblyDirectory()
{
    string codeBase = Assembly.GetExecutingAssembly().Location;
    UriBuilder uri = new UriBuilder(codeBase);
    string path = Uri.UnescapeDataString(uri.Path);
    return Path.GetDirectoryName(path);
}