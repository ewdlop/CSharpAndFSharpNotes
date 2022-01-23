public partial class Person
{
    public class BadCarEngine : IEngine
    {
        public int TopSpeed => 10;

        public int CurrentSpeed { get; set; }

        public void Run()
        {
            CurrentSpeed = 5;
        }
    }
}

