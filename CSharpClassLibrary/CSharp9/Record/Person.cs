namespace CSharpClassLibrary.CSharp9.Record
{
    public record Direction(float X, float Y);

    public record Person
    {
        public string LastName { get; }
        public string FirstName { get; }

        public Person(string first, string last) => (FirstName, LastName) = (first, last);

        public void Deconstruct(out object first, out object last)
        {
            first = FirstName;
            last = LastName;
        }
    }

    public record Doctor : Person
    {
        public string Specialization { get; }
        public Doctor(string first, string last, string spec) : base(first, last) => Specialization = spec;
    }

    public record Teacher(string FirstName, string LastName, string Subject) : Person(FirstName, LastName);
}
