using BlazorServerApp.Colors;
using MediatR;

namespace BlazorServerApp.Colors
{
    public class UpdateColorCommand : INotification
    {
        public string Color { get; set; }
    }
    public class UpdateColorEventArgs : EventArgs
    {
        public string Color { get; set; }
    }
}
namespace BlazorServerApp.Pages
{
    public partial class Counter : INotificationHandler<UpdateColorCommand>
    {
        public static event EventHandler<UpdateColorEventArgs> ColorChanged;
        public async Task Handle(UpdateColorCommand notification, CancellationToken cancellationToken)
        {
            ColorChanged?.Invoke(this, new UpdateColorEventArgs { Color = notification.Color });
        }
    }
}
namespace BlazorServerApp.Shared
{
    public partial class MainLayout : INotificationHandler<UpdateColorCommand>
    {
        public static event EventHandler<UpdateColorEventArgs> ColorChanged;
        public async Task Handle(UpdateColorCommand notification, CancellationToken cancellationToken)
        {
            ColorChanged?.Invoke(this, new UpdateColorEventArgs { Color = notification.Color });
        }
    }
}
