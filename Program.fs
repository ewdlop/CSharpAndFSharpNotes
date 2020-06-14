// Learn more about F# at http://fsharp.org

open System
open MyFSharpInterop.Color
open MyFSharpInterop.MathF

[<EntryPoint>]
let main argv =
    let helloWorld () = printfn "hello world"
    let throwAwayFirstInput x y = y
    Color.printColorName Color.Color.Red
    printfn "%i" (MathF.fib 5)
    helloWorld()
    0 // return an integer exit code
