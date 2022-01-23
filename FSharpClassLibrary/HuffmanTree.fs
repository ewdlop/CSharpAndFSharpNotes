module HuffmanTree

open System

type HuffmanTree = 
    | Leaf of char * float
    | Node of float * HuffmanTree * HuffmanTree

type HuffmanCoder(symbols: seq<char>, frequencies : seq<float>) =
   

    let huffmanTreeLeaves =    
        Seq.zip symbols frequencies
        |> Seq.toList
        |> List.map Leaf
        
    let frequency node =
        match node with
        | Leaf(_,p) -> p
        | Node(p,_,_) -> p    

    let rec buildCodingTree roots = 
        match roots |> List.sortBy frequency with
        | [] -> failwith "Cannot build a Huffman Tree for no inputs" 
        | [node] -> node
        | least::nextLeast::rest -> 
                   let combinedFrequency = frequency least + frequency nextLeast
                   let newNode = Node(combinedFrequency, least, nextLeast)
                   buildCodingTree (newNode::rest)
               
    let tree = buildCodingTree huffmanTreeLeaves
     
    let huffmanCodingTable = 
        let rec huffmanCodings tree = 
            match tree with
            | Leaf (c,_) -> [(c, [])]
            | Node (_, left, right) -> 
                let leftCodes = huffmanCodings left |> List.map (fun (c, code) -> (c, true::code))
                let rightCodes = huffmanCodings right |> List.map (fun (c, code) -> (c, false::code))
                List.append leftCodes rightCodes
        huffmanCodings tree 
        |> List.map (fun (c,code) -> (c,List.toArray code))
        |> Map.ofList


    let encode (str:string) = 
        let encodeChar c = 
            match huffmanCodingTable |> Map.tryFind c with
            | Some bits -> bits
            | None -> failwith "No frequency information provided for character '%A'" c
        str.ToCharArray()
        |> Array.map encodeChar
        |> Array.concat
       

    let decode bits =
        let rec decodeInner bitsRemain treeNode result = 
            match bitsRemain, treeNode with
            | [] , Node (_,_,_) -> failwith "Bits provided did not form a complete word"
            | [] , Leaf (c,_) ->  (c:: result) |> List.rev |> List.toArray
            | _  , Leaf (c,_) -> decodeInner bitsRemain tree (c::result)
            | b::rest , Node (_,l,r)  -> if b
                                         then decodeInner rest l result
                                         else decodeInner rest r result
        let bitsList = Array.toList bits
        new String (decodeInner bitsList tree [])
                 
    member coder.Encode source = encode source
    member coder.Decode source = decode source