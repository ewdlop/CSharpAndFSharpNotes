<Query Kind="Statements">
  <NuGetReference>Microsoft.SqlServer.DacFx</NuGetReference>
  <Namespace>Microsoft.SqlServer.TransactSql.ScriptDom</Namespace>
</Query>

//Assembly assembly = Assembly.GetAssembly(typeof(TSqlParser));
//assembly.GetTypes().Select(t => (t, hierachy: t.GetInheritanceHierarchy()))
//	.Where(h => h.hierachy.Contains(typeof(TSqlFragment))).OrderBy(h => h.t.Name).Dump();


string sql = """

""";

string sql3 = """

""";

string sql4 = """

""";

string sql5 = """

""";


TSqlParser parser = new TSql160Parser(true);
IList<ParseError> parseErrors;
//if(sql5.IndexOf("/") > 0)
//{
//	sql5 = sql5[..sql5.IndexOf("/")];
//}
TSqlFragment sqlFragment = parser.Parse(new StringReader(sql5), out parseErrors);
if (parseErrors.Count > 0) 
{
	parseErrors.Select(e => e.Message).Dump();
}

MyVistor visitor = new MyVistor();
sqlFragment.Accept(visitor);
//sqlFragment.Dump();
//visitor.VisitedNodes.Dump();
//visitor.Result.Dump();
//
//Assembly assembly = Assembly.GetAssembly(typeof(TSqlFragment));
//assembly.GetAllChildType(typeof(BooleanExpression)).Dump();
//
//static class TypeHelper
//{
//	public static IEnumerable<Type> GetInheritanceHierarchy(this Type type)
//	{
//		for (var current = type; current != null; current = current.BaseType)
//		{
//			yield return current;
//		}
//	}
//
//	public static IEnumerable<(Type, IEnumerable<Type>)> GetAllChildType(this Assembly assembly, Type type)
//	{
//		return assembly.GetTypes().Select(t => (t, hierachy: GetInheritanceHierarchy(t)))
//			.Where(h => h.hierachy.Contains(type)).OrderBy(h => h.t.Name);
//	}
//}





/// <summary>not thread safe</summary>
class MyVistor : TSqlFragmentVisitor
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
	//public override void ExplicitVisit(TSqlBatch node)
	//{
	//	//node.Dump();
	//	//PrintAndTrackVisit(node);
	//	//node.Statements.Dump();
	//	foreach(TSqlStatement statment in node.Statements)
	//	{
	//		TrackStatementExpression(statment);
	//	}
	//	base.ExplicitVisit(node);
	//}
	
	

	public override void ExplicitVisit(SearchedCaseExpression node)
	{
		//PrintAndTrackVisit(node);
		//node.Dump();
		_builder.Append("Case");
		_builder.AppendLine();
		foreach(SearchedWhenClause clause in node.WhenClauses)
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
		if(expression is SelectStatement select)
		{
			TrackQueryExpression(select.QueryExpression); 
		}
	}

	private void TrackQueryExpression(QueryExpression expression)
	{
		if (expression is BinaryQueryExpression binaryQuery)
		{
			TrackQueryExpression(binaryQuery.FirstQueryExpression);
			//binaryQuery.FirstQueryExpression.GetType().Dump();
			TrackQueryExpression(binaryQuery.SecondQueryExpression);
		}
		else if(expression is QuerySpecification query)
		{
			//query.Dump();
			foreach(SelectElement element in query.SelectElements)
			{
				TrackSelectExpression(element);
			}
			TrackFromExpression(query.FromClause);
			TrackWhereExpression(query.WhereClause);
			TrackGroupByExpression(query.GroupByClause);
			TrackHavingByExpression(query.HavingClause);
		}
	}
	private void TrackSelectExpression(SelectElement expression)
	{

	}

	public override void ExplicitVisit(WhereClause node)
	{
		TrackSearchConditionExpression(node.SearchCondition);
		base.ExplicitVisit(node);
	}
	

	

	private void TrackSearchConditionExpression(BooleanExpression expression)
	{
		//expression.Dump();
		TrackBooleanExpression(expression);
		//expression.ScriptTokenStream.Skip(expression.FirstTokenIndex).Take(expression.LastTokenIndex - (expression.FirstTokenIndex) + 1).Dump();
	}

	private void TrackBooleanExpression(BooleanExpression expression)
	{
		if(expression is BooleanBinaryExpression bbe)
		{
			TrackBooleanBinaryExpression(bbe);
		}
		else if(expression is BooleanParenthesisExpression bpe)
		{
			TrackBooleanParenthesisExpression(bpe);
		}
		else if(expression is ExistsPredicate ep)
		{
			TrackScalarSubQueryExpression(ep.Subquery);
		}
		else
		{
			(expression.GetType().Name,expression.ScriptTokenStream.Skip(expression.FirstTokenIndex).Take(expression.LastTokenIndex - expression.FirstTokenIndex + 1)).Dump();
		}
		//else if(expression is BooleanComparisonExpression bce)
		//{
		//	//TrackScalarExpression(bce.FirstExpression);
		//	//bce.ComparisonType.Dump();
		//	//TrackScalarExpression(bce.FirstExpression);
		//	bce.ScriptTokenStream.Skip(bce.FirstTokenIndex).Take(bce.LastTokenIndex - bce.FirstTokenIndex + 1).Dump();
		//}
		//else if(expression is InPredicate ip)
		//{
		//	ip.ScriptTokenStream.Skip(ip.FirstTokenIndex).Take(ip.LastTokenIndex - ip.FirstTokenIndex + 1).Dump();
		//}
	}

	private void TrackScalarSubQueryExpression(ScalarSubquery expression)
	{
		if (expression.QueryExpression is QuerySpecification qs)
		{
			TrackWhereClauseExpression(qs.WhereClause);
		}
		
	}

	private void TrackWhereClauseExpression(WhereClause expression)
	{
		TrackSearchConditionExpression(expression.SearchCondition);
	}

	private void TrackBooleanBinaryExpression(BooleanBinaryExpression expression)
	{
		TrackBooleanExpression(expression.FirstExpression);
		//expression.BinaryExpressionType.Dump();
		TrackBooleanExpression(expression.SecondExpression);
	}

	private void TrackBooleanParenthesisExpression(BooleanParenthesisExpression expression)
	{
		TrackBooleanExpression(expression.Expression);
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
		_builder.Append("When");
		//expression.ScriptTokenStream.Skip(expression.FirstTokenIndex-2).Take(expression.LastTokenIndex - (expression.FirstTokenIndex-2) + 1).Dump();
		if(expression is InPredicate predicate)
		{
			TrackInPredicateExpression(predicate);
		}
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
	private void TrackInPredicateExpression(InPredicate predicate)
	{
		TrackInPredicateExpressionExpression(predicate.Expression);
		_builder.Append("in");
		_builder.AppendWhiteSpace();
		_builder.Append("(");
		foreach(ScalarExpression expression in predicate.Values)
		{
			if(expression is StringLiteral literal)
			{
				_builder.AppendQuoteLiteral(literal.Value);	
			}
			if(expression != predicate.Values.Last())
			{
				_builder.Append(',');
			}
		}
		_builder.Append(")");
	}
	
	
	private void TrackInPredicateExpressionExpression(ScalarExpression expression)
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
//		Console.WriteLine(node.ToSqlString().Indent(2));
//
//		Console.WriteLine("¯".Multiply(40));
//
//		base.ExplicitVisit(node);
//	}
//}
