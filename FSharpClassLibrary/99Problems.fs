﻿module _99Problems

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
let flatten (ls :'a NestedList):('a List) =
    let rec loop acc ls =
        match ls with
        | Elem x -> x::acc
        | List xs -> List.foldBack (fun x acc -> loop acc x) xs acc
    loop [] ls
#nowarn "40"
let flatten' (ls :'a NestedList):('a List)=
    let rec loop = List.collect(function 
        | Elem x -> [x]
        | List xs -> loop xs)
    loop [ls]

let eliminateConsecutiveDuplicates xs = List.foldBack (fun x acc -> if List.isEmpty acc then [x] elif x = List.head acc then acc else x::acc) xs []
let eliminateConsecutiveDuplicates' xs = 
    match xs with
       | [] -> []
       | x::Xs -> List.fold(fun acc x -> if x = List.head acc then acc else x::acc) [x] xs |> List.rev

let packConsecutiveDuplicatesOfListElementsIntoSublists ls = 
    let collect x ls =
        match ls with
        | (y::ls)::xss when x= y -> (x::y::ls)::xss
        | xss -> [x]::xss
    List.foldBack collect ls []

let runLengthEncoding xs = xs |> packConsecutiveDuplicatesOfListElementsIntoSublists |> List.map(Seq.countBy id >> Seq.head >> fun (a,b)-> b,a)
let runLengthEncoding' xs = xs |> packConsecutiveDuplicatesOfListElementsIntoSublists |> List.map(fun xs -> List.length xs, List.head xs)
let runLengthEncoding'' xs =
    let collect x acc =
        match acc with
        | [] -> [(1,x)]
        | (n,y)::xs as acc ->
            if x = y then
                (n + 1,y)::xs
            else
                (1,x)::acc
    List.foldBack collect xs []
//11

type 'a Encoding = Multiple of int * 'a | Single of 'a
let modifiedRunLengthEncoding xs : 'a Encoding List = 
    let modifiedPackConsecutiveDuplicatesOfListElementsIntoSublist xs =
        let f (x, y, xs, xss, acc) = if x = y then (x::y::xs)::xss else [x]::acc
        let collect x xs =
            match xs with
            |[] -> failwith "Empty List"
            |[]::xss -> [x]::xss
            |(y::xs)::(xss) as acc -> f (x, y, xs, xss, acc)
        List.foldBack collect xs [[]]
    //let f (x,n) = if n = 1 then Single x else Multiple (n,x)
    xs |> modifiedPackConsecutiveDuplicatesOfListElementsIntoSublist |> List.map(Seq.countBy id >> Seq.head >> fun (x,n) -> if n = 1 then Single x else Multiple (n,x))

let decodeRunLengthEncoding (xs:'a Encoding List) =
    let expand xs =
        match xs with
            | Single x -> [x]
            | Multiple (n,x) -> List.replicate n x
    xs |> List.collect expand