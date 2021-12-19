using System;

namespace CSharpClassLibrary.CQRS.Commands
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
