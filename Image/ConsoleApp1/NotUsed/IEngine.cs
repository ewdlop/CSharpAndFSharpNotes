public partial class Person
{
    public interface IEngine
    {
        int CurrentSpeed { get; }
        int TopSpeed { get; }
        void Run();
    }
}

