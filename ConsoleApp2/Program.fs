open MyFSharpInterop.SymbolicCalculation.SymbolicCalculation
open Sandwich.MemoizationTailRecursion

let x1 = Derivative (Mul(Const(2.), X))
let x2 = Derivative (Const(2.))
let x3 = Derivative (Exp(X))
let x4 = Derivative (Mul(Const(2.), Exp(X)))

let y1 = FormatExpression x1
let y2 = FormatExpression x2
let y3 = FormatExpression x3
let y4 = FormatExpression x4


printfn "%s" y1
printfn "%s" y2
printfn "%s" y3
printfn "%s" y4

//let r = MemoizedTailRecursionFactorial 4
//printfn "%d" r

