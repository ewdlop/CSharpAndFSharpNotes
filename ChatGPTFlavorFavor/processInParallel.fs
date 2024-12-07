open System.Collections.Generic
open System.Threading.Tasks

// Define a union type to represent various types
type MyDataType =
    | Integer of int
    | String of string
    | Float of float

// Define an async sequence returning the union type
let asyncSequence: IAsyncEnumerable<MyDataType> = 
    asyncSeq {
        yield! [ async { return Integer 42 }
                 async { return String "Hello World" }
                 async { return Float 3.14 } ]
    }

// Process the async sequence in parallel using await and union type
let processInParallel asyncSeq =
    task {
        do! Parallel.ForEachAsync(
                asyncSeq,
                fun (item: MyDataType) _ -> task {
                    match item with
                    | Integer i -> 
                        do! Task.Delay(500) // Simulating async work
                        printfn "Integer: %d" i
                    | String s -> 
                        do! Task.Delay(300) // Simulating async work
                        printfn "String: %s" s
                    | Float f -> 
                        do! Task.Delay(200) // Simulating async work
                        printfn "Float: %f" f
                })
    }

// Run the parallel processing with async
processInParallel asyncSequence |> Async.AwaitTask |> Async.RunSynchronously
