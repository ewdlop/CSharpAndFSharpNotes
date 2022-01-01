using MediatR;

namespace BlazorServerApp.Data.CQRS;

public class UpdateColorCommand : INotification
{
    public string Color { get; set; }
}