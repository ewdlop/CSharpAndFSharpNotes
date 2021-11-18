namespace CSharpClassLibrary.Algebra.One.String
{
    public record StringGroupoid : IGroupoid<string>
    {
        public string Operation(string a, string b) => a + b;
    }
}
