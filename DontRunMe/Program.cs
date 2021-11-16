
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string filename = "CSharpAndFSharp/DontRunMe.cs"; //read in file to run from user
try
{
    if (FourtyTwo(filename, args))
        for (; ; );
    else
        return;
}
catch (OverflowException)
{
    for (; ; );
}

bool FourtyTwo(string filename, string[] args)
{
    bool noanswer = true;
    int i = 0;
    while(noanswer)
    {
        checked
        {
           i++;
        }
    }
    return true;
}