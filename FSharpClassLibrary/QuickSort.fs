open System.Math
open System.Threading.Tasks

module QuickSort = 
    let rec quicksort list =
        match list with
        | [] -> []                         
        | first::rest -> 
             let smaller,larger = List.partition ((>=) first) rest 
             quicksort smaller @ (first :: quicksort rest)
         
    let rec quicksortParallel list =
        match aList with
        | [] -> []
        | first :: rest ->
        let smaller, larger =  List.partition ((>=) first) rest
        let left = Task.Run(fun () -> quicksortParallel smaller)
        let right = Task.Run(fun () -> quicksortParallel larger)
        left.Result @ (firstElement :: right.Result)

    let depth = System.Math.Min((System.Math.Log System.Environment.ProcessorCount 2) + 4, 3)
    let rec quicksortParallelWithDepth depth list =
        match list with
        | [] -> []
        | first :: rest ->
        let smaller, larger = List.partition ((>=) first) rest
        if depth < 0 then
            let left = quicksortParallelWithDepth depth smaller
            let right = quicksortParallelWithDepth depth larger
            left @ (firstElement :: right)
        else
            let left = Task.Run(fun ()  -> quicksortParallelWithDepth (depth - 1) smaller)
            let right = Task.Run(fun () -> quicksortParallelWithDepth (depth - 1) larger)
            left.Result @ (firstElement :: right.Result)