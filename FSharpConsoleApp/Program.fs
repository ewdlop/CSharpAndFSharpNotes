// Learn more about F# at http://fsharp.org

open System
open MyFSharpInterop.Color
open MyFSharpInterop.MathF
open System.IO

let printTotalFileBytes path =
    async {
        let! bytes = File.ReadAllBytesAsync(path) |> Async.AwaitTask
        let fileName = Path.GetFileName(path)
        printfn "File %s has %d bytes" fileName bytes.Length
    }


[<EntryPoint>]
let main argv =
    let helloWorld () = printfn "hello world"
    let throwAwayFirstInput x y = y
    Color.printColorName Color.Color.Red
    printfn "%i" (MathF.fib 5)
    helloWorld()
    printTotalFileBytes "path-to-file.txt"
    |> Async.RunSynchronously
    0 // return an integer exit code
