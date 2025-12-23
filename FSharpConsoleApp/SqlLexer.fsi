module SqlLexer

open System   
open SqlParser   
open Lexing   /// Rule tokenize
val tokenize: lexbuf: LexBuffer<char> -> token
