using System;

namespace CSharpClassLibrary.CQRS.Commands
{
    public class RateProductCommand : ICommand
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public int ProductId { get; init; }
        public int Rating { get; init; }
        public int UserId { get; init; }
    }
}
