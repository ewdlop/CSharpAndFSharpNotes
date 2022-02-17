module QuickSort

let rec quicksort list =
    match list with
    | [] -> []                         
    | first::rest -> 
         let smaller,larger = List.partition ((>=) first) rest 
         List.concat [quicksort2 smaller; [first]; quicksort2 larger]