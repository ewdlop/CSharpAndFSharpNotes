using CSharpFunctionalExtensions;

namespace CSharpClassLibrary.CQRS.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }
}
