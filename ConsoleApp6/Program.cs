using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
//using Antlr4.Runtime;
//using Antlr4.Runtime.Tree;

string sql = "SELECT\r\nFirstName, LastName, Total\r\nFROM Orders\r\nWHERE Total = (SELECT MAX(Total) FROM Orders);";

//AntlrInputStream antlrInputStream = new AntlrInputStream(sql);
//SqlLexer lexer = new SqlLexer(antlrInputStream);
//CommonTokenStream tokens = new CommonTokenStream(lexer);
//SqlParser parser = new SqlParser(tokens);
//IParseTree tree = parser.select_statement();

ParseResult result = Parser.Parse(sql);

if (result.Errors.Any())
{
    foreach (Error error in result.Errors)
    {
        Console.WriteLine(error.Message);
    }
}
else
{
    IterateSqlNode(result.Script);
    //foreach (SqlBatch batch in result.Script.Batches)
    //{
    //    foreach (var statement in batch.Statements)
    //    {
    //        switch (statement)
    //        {
    //            case SqlSelectStatement selectStatement:
    //                Console.WriteLine(statement.GetType().Name);
    //                ProcessSelectStatement(selectStatement);
    //                break;
    //            default:
    //                Console.WriteLine("Unsupported statment. Printing inner XML");
    //                Console.WriteLine(statement.Xml);
    //                break;
    //        }
    //    }
    //}
}
static void IterateSqlNode(SqlCodeObject sqlCodeObject, int indent = 0)
{
    if (sqlCodeObject == null || sqlCodeObject.Children == null)
    {
        return;
    }
    foreach (var child in sqlCodeObject.Children)
    {
        Console.WriteLine($"{new string(' ', indent)}Sql:{child.Sql}/Type:{child.GetType().Name}");
        IterateSqlNode(child, indent + 2);
    }
}

static void ProcessSelectStatement(SqlSelectStatement selectStatement)
{
    SqlQuerySpecification query = (SqlQuerySpecification)selectStatement.SelectSpecification.QueryExpression;
    SqlSelectClause selectClause = query.SelectClause;
    Console.WriteLine($"Select columns {string.Join(", ", selectClause.SelectExpressions.Select(_ => _.Sql))}");
    SqlFromClause fromClause = query.FromClause;
    Console.WriteLine($"from tables {string.Join(", ", fromClause.TableExpressions.Select(_ => _.Sql))}");
    SqlWhereClause whereClause = query.WhereClause;
    Console.WriteLine($"where {whereClause.Expression.Sql}");
}