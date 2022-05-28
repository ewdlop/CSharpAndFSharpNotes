namespace MyFSharpInterop.MathF

module MathF =
    let rec fib n =
        match n with
        | 0 -> 0
        | 1 -> 1
        | _ -> fib (n - 1) + fib (n - 2)

    let sign x =
        match x with
        | 0 -> 0
        | x when x < 0 -> -1
        | x when x > 0 -> 1
        | _ -> failwith "somethign is wrong"
    
    //tail reucrison
    let fib2 n =
        let rec loop acc1 acc2 n =
            match n with
            | n when n = 0I -> acc1
            | n when n = 1I -> acc2
            | n -> loop acc2 (acc1 + acc2) (n - 1I)
        loop 0I 1I n

    let factorial n =
        let rec loop i acc =
            match i with
            | 0 | 1 -> acc
            | _ -> loop (i-1) (acc * i)
        loop n 1
