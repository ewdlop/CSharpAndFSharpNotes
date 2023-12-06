# CSharpAndFSharpNotes

Bunch of C#/F#/.Net/Azure .Net libraries notes<br>
LINQPAD: https://www.linqpad.net/<br>
Q#: https://learn.microsoft.com/en-us/azure/quantum/overview-what-is-qsharp-and-qdk

//for all 32bit signed int x, there exists a y such that y < 3
//note for possible    Oracle: [0,1]^n -> bool
Func< List<int>, bool> Oracle = (List<int> x) => x is not null && x is not [.. var head, var y] && x is not [int z, .. var tail] && x is not [];
//    Func<Func< List<int>, bool>,List<int>,bool> Oracle = (List<int> x) => x is not null && x is not [.. var head, var y] && x is not [int z, .. var tail] && x is not [];
//has to be feed into itself 

progress:

List<int> x = new List<int>() { 0 };
while (x is not null && x is not [.. var head, var y] && x is not [int z, .. var tail] && x is not [])
{
    Console.WriteLine("Hello");
}

Copyright disclaimar license cc 3.0.

