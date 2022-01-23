module QuickSort

let rec quicksort2 list =
    match list with
    | [] -> []                         
    | first::rest -> 
         let smaller,larger = List.partition ((>=) first) rest 
         List.concat [quicksort2 smaller; [first]; quicksort2 larger]