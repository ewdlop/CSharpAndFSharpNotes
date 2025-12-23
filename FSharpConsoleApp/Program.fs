// Learn more about F# at http://fsharp.org

open System
open System.Text
open MyFSharpInterop.Color
open MyFSharpInterop.MathF
open System.IO
open Sandwich
open Sql
open FSharp.Text.Lexing
open SqlParser
open SqlLexer

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

    // Asynchronous file reading example
    printfn "Make sure to replace \"path-to-file.txt\" with an actual file path on your system"

    printTotalFileBytes "path-to-file.txt"
    |> Async.RunSynchronously

    let x = "   
    SELECT x, y, z   
    FROM t1   
    LEFT JOIN t2   
    INNER JOIN t3 ON t3.ID = t2.ID   
    WHERE x = 50 AND y = 20   
    ORDER BY x ASC, y DESC, z   
    "   

    let lexbuf = LexBuffer<char>.FromString x 
    let y = SqlParser.start SqlLexer.tokenize lexbuf   
    printfn "%A" y   
  
    Console.WriteLine("(press any key)")   
    let _ = Console.ReadKey(true)
    0
