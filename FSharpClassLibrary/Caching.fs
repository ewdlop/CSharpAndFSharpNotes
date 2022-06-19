namespace FSharpClassLibrary

open System.Collections.Generic

module Caching = 
    let memoize func =
        let table = Dictionary<_,_>()
        fun x -> if table.ContainsKey(x) then table.[x]
                 else
                    let result = func x
                    table.[x] <- result
                    result
