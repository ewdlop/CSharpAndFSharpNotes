// Learn more about F# at http://fsharp.org

open System
open MyFSharpInterop.Color

[<EntryPoint>]
let main argv =
    Color.printColorName Color.Color.Red
    printfn "Hello World from F#!"
    0 // return an integer exit code
