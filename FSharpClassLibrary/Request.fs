module Request

type Request =
    { Name: string
      Email: string }

let validateName req = 
    match req.Name with
        | null -> Error "No name found."
        | "" -> Error "Name is empty"
        | "bananas" -> Error "Banans is not a name."
        | _ -> Ok req


let validateEmail req =
    match req.Email with
    | null -> Error "No email found."
    | "" -> Error "Email is empty."
    | s when s.EndsWith("bananas.com") -> Error "No email from bananas.com is allowed."
    | _ -> Ok req


let validateRequest reqResult = 
    reqResult
    |> Result.bind validateName
    |> Result.bind validateEmail

let req2 = { Name = "Phillip"; Email = "phillip@bananas.com" }
let res2 = validateRequest (Ok req2)

