using MediatR;

namespace BlazorServerApp.Colors
{
    public class UpdateColorCommand : INotification
    {
        public string Color { get; set; }
    }
}