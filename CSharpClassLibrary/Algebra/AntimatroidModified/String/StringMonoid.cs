namespace CSharpClassLibrary.Algebra.AntimatroidModified.String
{
    public record StringMonoid : StringSemigroup, IMonoid<string>
    {
        public string Identity
        {
            get { return string.Empty; }
        }
    }
}
