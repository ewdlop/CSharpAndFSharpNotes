namespace MyFSharpInterop.Person

module Person =
    type PersonName = 
        | FirstOnly of string
        | LastOnly of string
        | FirstLast of string * string
    let constructQuery (personName) =
        match personName with
        | FirstOnly(firstName) -> printf "May I call you %s?" firstName
        | LastOnly(lastName) -> printf "Are you Mr. or Ms. %s?" lastName
        | FirstLast(firstName, lastName) -> printf "Are you %s %s?" firstName lastName
    type Person = {First:string; Last:string}
    type Employee = 
        | Worker of Person
        | Manager of Employee list

    let jdoe = {First="John"; Last="Doe"}
    let worker = Worker jdoe