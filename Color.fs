namespace MyFSharpInterop.Color

module Color=
    type Color =
        | Red = 0
        | Green = 1
        | Blue = 2

    let printColorName color =
        match color with
        | Color.Red -> printfn "Red"
        | Color.Green -> printfn "Green"
        | Color.Blue -> printfn "Blue"
        | _ -> ()