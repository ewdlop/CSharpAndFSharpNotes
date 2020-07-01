namespace MyFSharpInterop.SymbolicCalculation

module SymbolicCalculation =
    type Expression =
        | X
        | Const of float
        | Neg of Expression
        | Add of Expression * Expression
        | Sub of Expression * Expression
        | Mul of Expression * Expression
        | Div of Expression * Expression
        | Quo of Expression * Expression
        | Pow of Expression * Expression
        | Log of Expression * Expression
        | Rec of Expression
        | Exp of Expression
        | Ln of Expression
        | Sin of Expression
        | Cos of Expression
        | Sec of Expression
        | Csc of Expression
        | Tan of Expression
        | Cot of Expression
        | Sinh of Expression
        | Cosh of Expression
        | Sech of Expression
        | Csch of Expression
        | Tanh of Expression
        | Coth of Expression

    let (|Op|_|) (e : Expression) =
        match e with
        | Add(e1, e2) -> Some(Add, e1, e2)
        | Sub(e1, e2) -> Some(Sub, e1, e2)
        | Mul(e1, e2) -> Some(Mul, e1, e2)
        | Quo(e1, e2) -> Some(Quo, e1, e2)
        | Div(e1, e2) -> Some(Div, e1, e2)
        | Pow(e1, e2) -> Some(Pow, e1, e2)
        | Log(e1, e2) -> Some(Log, e1, e2)
        | _ -> None
    
    let (|Func|_|) (e : Expression) =
        match e with
        | Rec(e) -> Some(Rec,e)
        | Exp(e) -> Some(Exp, e)
        | Ln (e) -> Some(Ln, e)
        | Sin(e) -> Some(Sin, e)
        | Cos(e) -> Some(Cos, e)
        | Sec(e) -> Some(Sec, e)
        | Csc(e) -> Some(Csc, e)
        | Tan(e) -> Some(Tan, e)
        | Cot(e) -> Some(Cot, e)
        | Sinh(e) -> Some(Sinh, e)
        | Cosh(e) -> Some(Cosh, e)
        | Sech(e) -> Some(Sech, e)
        | Csch(e) -> Some(Csch, e)
        | Tanh(e) -> Some(Tanh, e)
        | Coth(e) -> Some(Coth, e)
        | _ -> None

    let rec Simplify e : Expression =
        match e with
        | Add(Const(n1), Const(n2)) -> Const(n1 + n2)
        | Sub(Const(n1), Const(n2)) -> Const(n1 - n2)
        | Mul(Const(n1), Const(n2)) -> Const(n1 * n2)
        | Div(Const(n1), Const(n2)) -> Const(n1 / n2)
        | Neg(Const(0.)) -> Const(0.)
        | Neg(Neg(e)) -> e |> Simplify
        | Add(e, Const(0.)) -> e |> Simplify
        | Add(Const(0.), e) -> e |> Simplify
        | Add(Const(n), e) -> Add(e, Const(n)) |> Simplify
        | Add(e1, Neg(e2)) -> Sub(e1, e2) |> Simplify
        | Add(Neg(e1), e2) -> Sub(e2, e1) |> Simplify
        | Sub(e, Const(0.)) -> e |> Simplify
        | Sub(Const(0.), e) -> Neg(e) |> Simplify
        | Mul(e, Const(1.)) -> e |> Simplify
        | Mul(Const(1.), e) -> e |> Simplify
        | Mul(_e, Const(0.)) -> Const(0.)
        | Mul(Const(0.), _e) -> Const(0.)
        | Mul(e, Const(n)) -> Mul(Const(n), e) |> Simplify
        | Mul(Div(Const(n), e1), e2) -> Mul(Const(n), Div(e2, e1)) |> Simplify
        | Mul(e1, Div(Const(n), e2)) -> Mul(Const(n), Div(e1, e2)) |> Simplify
        | Mul(Neg(e1), e2) -> Neg(Mul(e1, e2)) |> Simplify
        | Mul(e1, Neg(e2)) -> Neg(Mul(e1, e2)) |> Simplify
        | Div(Const(0.), _e) -> Const(0.)
        | Div(e, Const(1.)) -> e |> Simplify
        | Div(Neg(e1), e2) -> Neg(Div(e1, e2)) |> Simplify
        | Div(e1, Neg(e2)) -> Neg(Div(e1, e2)) |> Simplify
        | Pow(Const(0.), _e) -> Const(0.)
        | Pow(Const(1.), _e) -> Const(1.)
        | Pow(_e, Const(0.)) -> Const(1.)
        | Pow(e, Const(1.)) -> e |> Simplify
        | Op (op, e1, e2)
            ->
            let e1s = Simplify e1
            let e2s = Simplify e2
            if e1s <> e1 || e2s <> e2 then
                op(Simplify e1, Simplify e2) |> Simplify
            else
                op(e1, e2)
        | _ -> e
    
    let rec Derivative e : Expression =
        let e' =
            match e with
            | X -> Const(1.)
            | Const(_n) -> Const(0.)
            | Neg(e) -> Neg(Derivative(e))
            | Add(e1, e2) -> Add(Derivative(e1), Derivative(e2)) //sum rule
            | Sub(e1, e2) -> Sub(Derivative(e1), Derivative(e2)) // difference rule
            | Mul(e1, e2) -> Add(Mul(Derivative(e1), e2), Mul(e1, Derivative(e2))) // product rule
            | Quo(e1,e2) -> Div(Sub(Mul(Derivative(e1),e2),Mul(e1,Derivative(e2))),Pow(e2, Const(2.))) //quotient rule,
            | Pow(e, Const(n)) -> Mul(Const(n), Pow(e, Const(n-1.))) //power rule
            | Pow(Const(n), e) -> Mul(Mul(Ln(Const(n)), Pow(Const(n), e)), Derivative(e))
            | Rec(e) -> Derivative(Div(Const(1.), e)) // reciprocal rule
            | Div(Const(1.), e) -> Neg(Div(Derivative(e), Pow(e, Const(2.))))
            | Log(X,Const(n)) -> Div(Const(1.),Mul(X,Ln(Const(n))))
            | Exp(X) -> Exp(X)
            | Ln (X) -> Div(Const(1.), X)
            | Sin(X) -> Cos(X)
            | Cos(X) -> Neg(Sin(X))
            | Sec(X) -> Mul(Tan(X), Sec(X))
            | Csc(X) -> Neg(Mul(Cot(X),Csc(X)))
            | Tan(X) -> Pow(Sec(X),Const(2.))
            | Cot(X) -> Neg(Pow(Csc(X),Const(2.)))
            | Sinh(X) -> Cosh(X)
            | Cosh(X) -> Sinh(X)
            | Sech(X) -> Neg(Mul(Tanh(X),Sech(X)))
            | Cosh(X) -> Neg(Mul(Coth(X),Csch(X)))
            | Tanh(X) -> Pow(Sech(X),Const(2.))
            | Coth(X) -> Neg(Pow(Csch(X),Const(2.)))
            | Func(e1, e2) -> // chain rule
                let f' = Derivative(e1(X))
                let g' = Derivative(e2)
                match f' with
                | Func(f', _e) -> Mul(f'(e2), g')
                | Op (op, e1, e2) -> Mul(op(e1, e2), g')
                | _ -> failwith(sprintf "Unable to match compound function [%A]" f')
            | _ -> failwith(sprintf "Unable to match expression [%A]" e)
        Simplify e'

    let OperatorName (e: Expression) : string =
       match e with
       | Add(e1, e2) -> "+"
       | Sub(e1, e2) -> "-"
       | Mul(e1, e2) -> "*"
       | Div(e1, e2) -> "/"
       | Pow(e1, e2) -> "^"
       | _ -> failwith(sprintf "Unrecognized operator [%A]" e)

    let FunctonName (e: Expression) (a : string) : string =
           match e with
           | Exp(x) -> sprintf "e^(%s)" a
           | Ln(x) -> sprintf "ln(%s)" a
           | Sin(x) -> sprintf "sin(%s)" a
           | Cos(x) -> sprintf "cos(%s)" a
           | Sec(x) -> sprintf "sec(%s)" a
           | Csc(x) -> sprintf "csc(%s)" a
           | Tan(x) -> sprintf "tan(%s)" a
           | Cot(x) -> sprintf "cot(%s)" a
           | Sinh(x) -> sprintf "sinh(%s)" a
           | Cosh(x) -> sprintf "cosh(%s)" a
           | Sech(x) -> sprintf "sech(%s)" a
           | Csch(x) -> sprintf "csch(%s)" a
           | Tanh(x) -> sprintf "tanh(%s)" a
           | Coth(x) -> sprintf "coth(%s)" a
           | _ -> failwith(sprintf "Unrecognized function [%A]" e)

    let FormatExpression x =
        let rec FormatSubExpression (outer : Expression option, inner : Expression) : string =
            match inner with
            | X -> "x"
            | Const(n) -> sprintf "%f" n
            | Neg x -> sprintf "-%s" (FormatSubExpression(Some(inner), x))
            | Op(op, e1, e2) ->
                let s = FormatSubExpression(Some(inner), e1) + " " + OperatorName(inner) + " " + FormatSubExpression(Some(inner), e2)
                match outer with
                | None -> s
                | _ -> "(" + s + ")"
            | Func(f, e) -> FunctonName(inner) (FormatSubExpression(None, e))
        FormatSubExpression(None, x)

    let t1 = FormatExpression(Mul(Const(2.), X))
    let t2 = FormatExpression(Mul(Const(3.), Mul(Const(2.), X)))
    let t3 = FormatExpression(Mul(Mul(Const(2.), X), Const(3.)))
    let t4 = FormatExpression(Mul(Add(X, Const(2.)), Const(3.)))
    let t5 = FormatExpression(Neg(Mul(Const(2.), X)))
    let t6 = FormatExpression(Sin(X))