public partial class Person
{
    public string? Name { get; set; }
    public int Age { get; set; }

    public void Test()
    {
        IList<Person> list = new List<Person>
        {
            new Person { Name = "Raymond", Age = 1 },
            new Person { Name = "Lei", Age = 11 }
        };
        IList<string> Names = list.Where(p => p.Age > 10).Select(p => p.Name)
            .ToList();
    }
}

