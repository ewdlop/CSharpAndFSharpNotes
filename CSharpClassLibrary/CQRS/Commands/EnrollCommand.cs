using CSharpFunctionalExtensions;

namespace CSharpClassLibrary.CQRS.Commands
{
    public sealed partial record EnrollCommand : ICommand
    {
        public long Id { get; init; }
        public string Course { get; init; }
        public string Grade { get; init; }
    }
}
