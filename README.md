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

//i am not convincing myself yet

IEnumerable<int> Machine(int[] x)
{
    while(x is not null && x is not [.. var head, var y] && x is not [int z, .. var tail] && x is not [])
    {
        yield return 1;
    }
    yield return 0;
}
Copyright disclaimar license cc 3.0.

