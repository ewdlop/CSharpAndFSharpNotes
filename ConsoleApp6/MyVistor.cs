using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Text;
/// <summary>not thread safe</summary>
public class MyVistor : TSqlFragmentVisitor
{
    protected Stack<(TSqlFragment, Type)> _visitedNodes = new();
    public IReadOnlyCollection<(TSqlFragment, Type)> VisitedNodes => _visitedNodes;
    private StringBuilder _builder = new StringBuilder();

    public void Clear()
    {
        _visitedNodes.Clear();
        _builder.Clear();
    }
    /// <summary>not fully parsable. for debug only</summary>
    public string Result => _builder.ToString();
    public override void ExplicitVisit(TSqlBatch node)
    {
        //node.Dump();
        PrintAndTrackVisit(node);
        node.Statements.Dump();
        foreach (TSqlStatement statment in node.Statements)
        {
            TrackStatementExpression(statment);
        }
        base.ExplicitVisit(node);
    }


    public override void ExplicitVisit(SearchedCaseExpression node)
    {
        PrintAndTrackVisit(node);
        _builder.Append("Case");
        _builder.AppendLine();
        foreach (SearchedWhenClause clause in node.WhenClauses)
        {
            TrackWhenExpression(clause.WhenExpression);
            TrackThenExpression(clause.ThenExpression);
        }
        TrackElseExpression(node.ElseExpression);
        _builder.AppendLine();
        base.ExplicitVisit(node);
    }

    private void TrackStatementExpression(TSqlStatement expression)
    {
        if (expression is SelectStatement select)
        {
            TrackQueryExpression(select.QueryExpression);
        }
    }

    private void TrackQueryExpression(QueryExpression expression)
    {
        if (expression is BinaryQueryExpression binaryQuery)
        {
            TrackQueryExpression(binaryQuery.FirstQueryExpression);
            binaryQuery.FirstQueryExpression.GetType().Dump();
            TrackQueryExpression(binaryQuery.SecondQueryExpression);
        }
        else if (expression is QuerySpecification query)
        {
            foreach (SelectElement element in query.SelectElements)
            {
                TrackSelectExpression(element);
            }
            TrackFromExpression(query.FromClause);
            TrackWhereExpression(query.WhereClause);
            TrackGroupByExpression(query.GroupByClause);
            TrackHavingByExpression(query.HavingClause);
            TrackOrderByExpression(query.OrderByClause);
        }
    }
    private void TrackSelectExpression(SelectElement expression)
    {

    }

    private void TrackFromExpression(FromClause expression)
    {

    }

    private void TrackWhereExpression(WhereClause expression)
    {

    }

    private void TrackGroupByExpression(GroupByClause expression)
    {
    }

    private void TrackHavingByExpression(HavingClause expression)
    {
    }

    private void TrackOrderByExpression(OrderByClause expression)
    {
    }

    private void TrackWhenExpression(BooleanExpression expression)
    {

    }
    private void TrackElseExpression(ScalarExpression expression)
    {
        if (expression is StringLiteral elseLiteral)
        {
            _builder.Append("ELSE");
            _builder.AppendWhiteSpace();
            _builder.AppendQuoteLiteral(elseLiteral.Value);
            _builder.AppendWhiteSpace();
        }
    }

    private void TrackThenExpression(ScalarExpression expression)
    {
        if (expression is StringLiteral thenliteral)
        {
            _builder.AppendWhiteSpace();
            _builder.Append("Then");
            _builder.AppendWhiteSpace();
            _builder.AppendQuoteLiteral(thenliteral.Value);
            _builder.AppendLine();
        }
    }

    private void TrackPredicateExpressionExpression(ScalarExpression expression)
    {
        if (expression is ColumnReferenceExpression column)
        {
            _builder.AppendWhiteSpace();
            foreach (Identifier identifier in column.MultiPartIdentifier.Identifiers)
            {
                if (identifier.QuoteType == QuoteType.SquareBracket)
                {
                    _builder.AppenBracketIdentfier(identifier.Value);
                }
                else if (identifier.QuoteType == QuoteType.DoubleQuote)
                {
                    _builder.AppendDoubleQuoteLiteral(identifier.Value);
                }
                else if (identifier.QuoteType == QuoteType.NotQuoted)
                {
                    _builder.Append(identifier.Value);
                }
            }
            _builder.AppendWhiteSpace();
        }
    }

    protected void PrintAndTrackVisit<T>(T node) where T : TSqlFragment
    {
        _visitedNodes.Push((node, node.GetType()));
        //$"Visit {node.GetType().Name}".Dump();
        //node.Dump();
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