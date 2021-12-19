using System;

namespace CSharpClassLibrary.CQRS.Commands
{
    public sealed partial record EnrollCommand : ICommand
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string Course { get; init; }
        public string Grade { get; init; }
    }
}
