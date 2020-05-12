namespace MyFSharpInterop.Number

module Number =
    type TwoNumbers = int * int
    type ThreeNumbers = int * int * int

    let f a b x = a * x + b
    let productOfTwo ((x1,x2): TwoNumbers) = x1 * x2
    let sumOfThree ((x1,x2,x3): ThreeNumbers) = x1 + x2 + x3