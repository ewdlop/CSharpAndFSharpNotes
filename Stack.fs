namespace MyFSharpInterop.Stack

module Stack =
    type 'a stack =
        | EmptyStack
        | StackNode of 'a * 'a stack
    let stack = StackNode(1, StackNode(2, StackNode(3, StackNode(4, StackNode(5, EmptyStack)))))