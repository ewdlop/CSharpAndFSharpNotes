//https://en.wikibooks.org/wiki/F_Sharp_Programming/Lexing_and_Parsing

module Sql

type value =
    | Int of int
    | Float of float
    | String of string

type dir =
    | Asc
    | Desc

type op = Eq | Gt | Ge | Lt | Le

type order = string * dir

type where = 
    | Cond of value * op * value
    | And of where * where
    | Or of where * where

type joinType = Inner | Left | Right | Full

type join = string * joinType * where option// table, type, condition

type sqlStatement =   
    {
        Table : string;
        Columns : string list;
        Joins : join list;
        Where : where option;
        OrderBy : order list;
    }