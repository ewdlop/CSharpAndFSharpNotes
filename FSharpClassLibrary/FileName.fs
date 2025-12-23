module FSharpClassLibrary.FileName

//type Expression = 
//    | Value of LiteralAttribute
//    | Variable of VarName
//    | MethodInvoke of MemberName * Arg list
//    | PropertyGet of MemberName
//    | Cast of TypeName * Expression
//    | InfixOp of Expression * string * Expression
//    | PrefixOp of string * Expression
//    | PostfixOp of Expression * string
//    | TernaryOp of Expression * Expression * Expression


//type Statement =
//    | Assignment of Init
//    | PropertySet of MemberName * Expression
//    | Action of Expression
//    | If of Expression * Block
//    | IfElse of Expression * Block * Block
//    | Switch of Expression * Case list
//    | For of Init list * Condition * Iterator list * Block
//    | ForEach of Define * Expression * Block
//    | While of Expression * Block
//    | DoWhile of Block * Expression
//    | Throw of Expression
//    | Try of Block
//    | Catch of TypeName * Block
//    | Finally of Block
//    | Lock of Expression * Block    
//    | Using of Expression * Block
//    | Label of LabelName
//    | Goto of LabelName
//    | Break
//    | Continue
//    | Return of Expression

//// Modifiers
//type Access = Public | Private | Protected | Internal
//type Modifier = Static | Sealed | Override | Virtual | Abstract
//type ParamType = ByValue | ByRef | Out | Params 

//// Members
//type Member =
//    | Field of Access * Modifier option * IsReadOnly * 
//               ReturnType * Name * Expression option
//    | Property of MemberInfo * Block option * Block option
//    | Method of MemberInfo * Param list * Block
//    | Constructor of Access * Modifier option * Name * Param list * 
//                     PreConstruct option * Block

//type CSharpType = 
//    | Class of Access * Modifier option * Name * Implements * Members
//    | Struct of Access * Name * Member list
//    | Interface of Access * Name * Implements * Member list
//    | Enum of Access * TypeName * EnumValue list
//    | Delegate of Access * Name * ReturnType * Param list    

//type Import = 
//    | Import of Name list
//    | Alias of Name * Name list
//type NamespaceScope =
//    | Namespace of Import list * Name list * NamespaceScope list
//    | Types of Import list * CSharpType list