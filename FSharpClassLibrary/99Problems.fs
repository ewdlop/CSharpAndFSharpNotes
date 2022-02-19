module _99Problems

// Problem 1 : Find the last element of a list.
let rec findLastElement xs =
    match xs with
        | []-> failwith "empty list"
        | [x] -> x
        | _::xs -> findLastElement xs
let rec findLastElement' xs = xs |> List.rev |> List.head
let findLastElement'' xs = List.reduce(fun _ _x -> _x) xs

// Problem 2 : Find the last but one element of a list.
let rec findPenultimateElement xs =
    match xs with
        | [] -> failwith "empty list"
        | [_]-> failwith "singleton list"
        | [x;_]-> x
        | _::xs -> findPenultimateElement xs
let findPenultimateElement' xs = xs |> List.rev |> List.tail |> List.head
let findPenultimateElement'' xs = 
    let flip f a b = f b a 
    xs |> List.rev |> flip (List.item 1)

// Problem 3 : Find the K'th element of a list. The first element in the list is number 1.
let rec findk'thElement xs n =
    match (xs, n) with
        | ([], _) -> failwith "empty list"
        | (x::_, 1) -> x
        | (_::xs, n) -> findk'thElement xs n-1
let rec findk'thElement' xs n = List.item xs n-1

// Problem 4 : Find the number of elements of a list.
let findNumberOfElements xs =
    let rec length acc xs =
            match xs with
            | [] -> acc
            | _::xs -> length (acc+1) xs
    length 0 xs
let findNumberOfElements' xs = List.length xs
let findNumberOfElements'' xs = xs |> List.sumBy(fun _-> 1)

// Problem 5 : Reverse a list.

let reverse xs =
    let rec rev acc xs =
        match xs with
        | [] ->acc
        | x::xs -> rev (x::acc) xs
    rev [] xs
let reverse' xs = List.fold (fun acc x -> x::acc) [] xs
let reverse'' xs = List.rev xs

// Problem 6 : Find out whether a list is a palindrome.
let IsPalindrome xs = xs = List.rev xs

// Problem 7 : Flatten a nested list structure.
type 'a NestedList = List of 'a NestedList list | Elem of 'a

let flatten (ls :'a NestedList):('a List) =
    let rec loop acc ls =
        match ls with
        | Elem x -> x::acc
        | List xs -> List.foldBack (fun x acc -> loop acc x) xs acc
    loop [] ls
#nowarn "40"
let flatten' (ls :'a NestedList):('a List)=
    let rec loop = List.collect(fun x -> 
        match x with
        | Elem x -> [x]
        | List xs -> loop xs)
    loop [ls]

// Problem 8 : Eliminate consecutive duplicates of list elements.
let eliminateConsecutiveDuplicates xs = (xs, []) ||> List.foldBack (fun x acc -> if List.isEmpty acc then [x] elif x = List.head acc then acc else x::acc)
let eliminateConsecutiveDuplicates' xs = 
    match xs with
       | [] -> []
       | x::xs -> List.fold (fun acc x -> if x = List.head acc then acc else x::acc) [x] xs |> List.rev

// Problem 10 : Run-length encoding of a list.
let packConsecutiveDuplicatesOfListElementsIntoSublists ls = 
    let collect x ls =
        match ls with
        | (y::ls)::xss when x = y -> (x::y::ls)::xss
        | xss -> [x]::xss
    List.foldBack collect ls []
let runLengthEncoding xs = xs |> packConsecutiveDuplicatesOfListElementsIntoSublists |> List.map(Seq.countBy id >> Seq.head >> fun (a,b)->b,a)
let runLengthEncoding' xs = xs |> packConsecutiveDuplicatesOfListElementsIntoSublists |> List.map(fun xs -> List.length xs, List.head xs)
let runLengthEncoding'' xs =
    let collect x (acc:(int * 'b) list) =
        match acc with
        | [] -> [(1,x)]
        | (n,y)::xs as acc ->
            if x = y then
                (n + 1,y)::xs
            else
                (1,x)::acc
    List.foldBack collect xs []

// Problem 11 : Modified run-length encoding.
type 'a Encoding = Multiple of int * 'a | Single of 'a
let modifiedRunLengthEncoding xs : 'a Encoding list = 
    let modifiedPackConsecutiveDuplicatesOfListElementsIntoSublist xs =
        let f (x, y, xs, xss, acc) = if x = y then (x::y::xs)::xss else [x]::acc
        let collect x (xs:'c list list) =
            match xs with
            | [] -> failwith "Empty List"
            | []::xss -> [x]::xss
            | (y::xs)::(xss) as acc -> f (x, y, xs, xss, acc)
        List.foldBack collect xs [[]]
    //let f (x,n) = if n = 1 then Single x else Multiple (n,x)
    xs |> modifiedPackConsecutiveDuplicatesOfListElementsIntoSublist |> List.map(Seq.countBy id >> Seq.head >> fun (x,n) -> if n = 1 then Single x else Multiple (n,x))

// Problem 12 : Decode a run-length encoded list.
let decodeRunLengthEncoding (xs:'a Encoding list) =
    let expand xs =
        match xs with
            | Single x -> [x]
            | Multiple (n,x) -> List.replicate n x
    xs |> List.collect expand

// Problem 13 : Decode a run-length encoded list.
let runLengthEncodingDirect xs =
    let collect x (xs:'a Encoding list) =
        match xs with
            | [] -> [Single x]
            | Single y::xs when x = y -> Multiple (2,x)::xs
            | Single _::_ as xs -> Single x::xs
            | Multiple (n,y)::xs when y=x -> Multiple (n + 1,x)::xs
            | xs -> Single x::xs
    (xs, []) ||> List.foldBack collect 

// Problem 14 : Duplicate the elements of a list.
let duplciateTheElements xs = xs |> List.map (fun x-> [x;x]) |> List.concat
let rec duplciateTheElements' xs =
    match xs with
    | [] -> []
    | x::xs -> x::x::duplciateTheElements xs
let duplciateTheElements'' xs = [for x in xs do yield x;yield xs]
let duplciateTheElements''' xs = xs |> List.collect (fun x-> [x;x])
let duplciateTheElements'''' xs = (xs,[]) ||> List.foldBack (fun x xs-> x::x::xs)
let duplicateTheElements''''' xs = ([],xs) ||> List.fold (fun xs x -> xs @ [x;x])
let duplicateTheElements'''''' xs = xs |> List.collect (List.replicate 2)

// Problem 15 : Replicate the elements of a list a given number of times.
let replicateTheELementsNTimes xs n = xs |> List.collect( List.replicate n)
let replicateTheELementsNTimes'' xs n = [for x in xs do for _=1 to n do yield x]

// Problem 16 : Drop every N'th element from a list.
let dropEveryNthElement xs n = xs |> List.mapi(fun i x -> (i + 1,x)) |> List.filter(fun (i, _) -> i % n <> 0) |> List.map snd
let dropEveryNthElement' xs n = 
    let rec drop (xs:'b list) (count:int) =
        match (xs,count) with
            |[],_ -> []
            |_::xs,1 -> drop xs n
            |x::xs,_ -> x::drop xs (count-1)
    drop xs n