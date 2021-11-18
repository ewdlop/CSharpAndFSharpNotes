namespace CSharpClassLibrary.Algebra.AntimatroidModified.String
{
    public record StringGroupoid : IGroupoid<string>
    {
        public string Operation(string a, string b) => a + b;
    }
}
