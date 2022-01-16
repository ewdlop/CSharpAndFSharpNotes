// Learn more about F# at http://fsharp.org

open System
open MyFSharpInterop.Color
open MyFSharpInterop.MathF
open System.IO
open Sandwich

let printTotalFileBytes path =
    async {
        let! bytes = File.ReadAllBytesAsync(path) |> Async.AwaitTask
        let fileName = Path.GetFileName(path)
        printfn "File %s has %d bytes" fileName bytes.Length
    }


[<EntryPoint>]
let main argv =
    let r = MemoizationTailRecursion.MemoizedTailRecursionFactorial 4
    printfn "%d" r
    let helloWorld () = printfn "hello world"
    let throwAwayFirstInput x y = y
    Color.printColorName Color.Color.Red
    printfn "%i" (MathF.fib 5)
    helloWorld()
    printTotalFileBytes "path-to-file.txt"
    |> Async.RunSynchronously
    0 // return an integer exit code
