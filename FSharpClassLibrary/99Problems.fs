module _99Problems

let rec findLastElement xs =
    match xs with
        | []-> failwith "empty list"
        | [x] -> x
        | _::xs -> findLastElement xs
let rec findLastElement' xs = xs |> List.rev |> List.head
let findLastElement'' xs = List.reduce(fun acc _x -> _x) xs

let rec findPenultimateElement xs =
    match xs with
        | [] -> failwith "empty list"
        | [x]-> failwith "singleton list"
        | [x;_]-> x
        | _::xs -> findPenultimateElement xs
let findPenultimateElement' xs = xs |> List.rev |> List.tail |> List.head
let findPenultimateElement'' xs = 
    let flip f a b = f b a 
    xs |> List.rev |> flip (List.item 1)

let rec findk'thElement xs n =
    match (xs, n) with
        | ([], _) -> failwith "empty list"
        | (x::_, 1) -> x
        | (_::xs, n) -> findk'thElement xs n-1
let rec findk'thElement' xs n = List.item xs n-1

let findNumberOfElements xs =
    let rec length acc xs =
            match xs with
            | [] -> acc
            | _::xs -> length (acc+1) xs
    length 0 xs
let findNumberOfElements' xs = List.length xs
let findNumberOfElements'' xs = xs |> List.sumBy(fun _-> 1)

let reverse xs =
    let rec rev acc xs =
        match xs with
        | [] ->acc
        | x::xs -> rev (x::acc) xs
    rev [] xs
let reverse' xs = List.fold (fun acc x -> x::acc) [] xs
let reverse'' xs = List.rev

let IsPalindrome xs = xs = List.rev xs

type 'a NestedList = List of 'a NestedList list | Elem of 'a
let flatten ls =
    let rec loop acc ls =
        match ls with
        | Elem x -> x::acc
        | List xs -> List.foldBack (fun x acc -> loop acc x) xs acc
    loop [] ls
let flatten' ls =
    let rec loop = List.collect(function 
    | Elem x -> [x]
    | List xs -> loop xs)
    loop [ls]

//let eliminateConsecutiveDuplicates xs;

let packConsecutiveDuplicatesOfListElementsIntoSublists ls = 
    let collect x ls =
        match ls with
        | (y::ls)::xss when x= y -> (x::y::ls)::xss
        | xss -> [x]::xss
    List.foldBack collect ls []