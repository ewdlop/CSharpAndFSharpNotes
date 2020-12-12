namespace MyFSharpInterop.LambdaCalculus

(*
Examples:
  \x.x                           -----> \x.x
  x                              -----> error!!!
  (\x.x \y.y)                    -----> \y.y
  ((\x.\y.x  \a.(a a))   \b.b)  -----> \a.(a a)
*)


module LambdaCalculus =
    type Token =
      | LParen
      | RParen
      | Lambda
      | Dot
      | Variable of char

    let alphabet = List.ofSeq "abcdefghijklmnopqrstuvwxyz"

    let rec tokenize (text: char list) =
      match text with
        | [] -> []
        | '('::rest -> LParen::tokenize rest
        | ')'::rest -> RParen::tokenize rest
        | '.'::rest -> Dot::tokenize rest
        | '\\'::rest -> Lambda::tokenize rest
        | c::rest ->
          (if List.contains c alphabet
          then [Variable c]
          else []) @ tokenize rest

    type Term =
      | VariableT of char
      | LambdaT of char*Term
      | ClosureT of char*Term*Env
      | ApplicationT of Term*Term

    and Env = (char*Term) list

    let rec parseSingle (tokens: Token list): (Term*Token list) =
      match tokens with
        | (Variable name::rest) -> VariableT name, rest
        | (Lambda::Variable arg::Dot::bodyCode) ->
          let body, rest = parseSingle bodyCode

          LambdaT (arg, body), rest

        | LParen::code ->
          let fn, afterFirst = parseSingle code
          let value, afterValue = parseSingle afterFirst

          match afterValue with
            | RParen::rest -> ApplicationT (fn, value), rest
            | _ ->
              failwith "Expected )"
        | _ ->
          failwith "Bad parse"


    let parse (tokens: Token list) = 
      fst <| parseSingle tokens

    let rec evalInEnv (env: Env) (term: Term): Term =
      match term with
        | VariableT name ->
          match List.tryFind (fun (aName, term) -> aName = name) env with
            | Some (_, term) -> term
            | None -> failwith "Couldn't find a term by name"
        | LambdaT (arg, body) ->
          ClosureT (arg, body, env)
        | ApplicationT (fn, value) ->
          match evalInEnv env fn with
            | ClosureT (arg, body, closedEnv) ->
              let evaluatedValue = evalInEnv env value

              let newEnv = (arg, evaluatedValue)::closedEnv @ env

              evalInEnv newEnv body
            | _ ->
              failwith "Cannot apply something given"
        | closure -> closure

    let eval (term: Term): Term =
      evalInEnv [] term

    let rec pretty (term: Term): char list =
      match term with
        | VariableT name -> [name]
        | LambdaT (arg, body) -> ['\\'; arg; '.'] @ pretty body
        | ClosureT (arg, body, _) -> ['\\'; arg; '.'] @ pretty body
        | ApplicationT (fn, value) -> ['('] @ pretty fn @ [' '] @ pretty value @ [')']

    let interp: char list -> char list =
      tokenize
        >> parse
        >> eval
        >> pretty

    let interpString: string -> string =
      List.ofSeq
        >> interp
        >> List.map string
        >> String.concat ""
