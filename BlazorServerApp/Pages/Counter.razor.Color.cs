using BlazorServerApp.Colors;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace BlazorServerApp.Pages
{
    public partial class Counter : INotificationHandler<UpdateColorCommand>
    {
        [Inject]
        private IMediator Mediator { get; init; } 
        public static event EventHandler<UpdateColorEventArgs> ColorChanged;
        public async Task Handle(UpdateColorCommand notification, CancellationToken cancellationToken)
        {
            ColorChanged?.Invoke(this, new UpdateColorEventArgs { Color = notification.Color });
        }
    }
}