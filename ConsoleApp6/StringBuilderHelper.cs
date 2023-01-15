using System.Text;

public static class StringBuilderHelper
{
    public static StringBuilder AppendWhiteSpace(this StringBuilder bulider)
    {
        return bulider.Append(" ");
    }

    public static StringBuilder AppendQuoteLiteral(this StringBuilder bulider, string value)
    {
        return bulider.Append($"\'{value}\'");
    }

    public static StringBuilder AppendDoubleQuoteLiteral(this StringBuilder bulider, string value)
    {
        return bulider.Append($"\"{value}\"");
    }

    public static StringBuilder AppenBracketIdentfier(this StringBuilder bulider, string value)
    {
        return bulider.Append($"[{value}]");
    }
}


//class OwnVisitor : TSqlFragmentVisitor
//{
//	public override void ExplicitVisit(SelectStatement node)
//	{
//		QuerySpecification querySpecification = node.QueryExpression as QuerySpecification;
//
//		FromClause fromClause = querySpecification.FromClause;
//		// There could be more than one TableReference!
//		// TableReference is not sure to be a NamedTableReference, could be as example a QueryDerivedTable
//		NamedTableReference namedTableReference = fromClause.TableReferences[0] as NamedTableReference;
//		TableReferenceWithAlias tableReferenceWithAlias = fromClause.TableReferences[0] as TableReferenceWithAlias;
//		string baseIdentifier = namedTableReference?.SchemaObject.BaseIdentifier?.Value;
//		string schemaIdentifier = namedTableReference?.SchemaObject.SchemaIdentifier?.Value;
//		string databaseIdentifier = namedTableReference?.SchemaObject.DatabaseIdentifier?.Value;
//		string serverIdentifier = namedTableReference?.SchemaObject.ServerIdentifier?.Value;
//		string alias = tableReferenceWithAlias.Alias?.Value;
//		Console.WriteLine("From:");
//		Console.WriteLine($"  {"Server:",-10} {serverIdentifier}");
//		Console.WriteLine($"  {"Database:",-10} {databaseIdentifier}");
//		Console.WriteLine($"  {"Schema:",-10} {schemaIdentifier}");
//		Console.WriteLine($"  {"Table:",-10} {baseIdentifier}");
//		Console.WriteLine($"  {"Alias:",-10} {alias}");
//
//
//
//		// Example of changing the alias:
//		//(fromClause.TableReferences[0] as NamedTableReference).Alias = new Identifier() { Value = baseIdentifier[0].ToString() };
//
//		Console.WriteLine("Statement:");
//		Console.WriteLine(node.To