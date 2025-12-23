module SqlLexer

open System   
open SqlParser   
open FSharp.Text.Lexing   /// Rule tokenize
val tokenize: lexbuf: LexBuffer<char> -> token
