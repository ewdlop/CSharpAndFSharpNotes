namespace CSharpClassLibrary.CQRS.Queries
{

    public sealed partial class GetListQuery
    {
        public sealed class QueriesConnectionString
        {
            public string Value { get; }

            public QueriesConnectionString(string value)
            {
                Value = value;
            }
        }
    }
}
