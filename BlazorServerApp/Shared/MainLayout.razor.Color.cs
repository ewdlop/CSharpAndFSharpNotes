using BlazorServerApp.Colors;
using MediatR;
namespace BlazorServerApp.Shared
{
    public partial class MainLayout : INotificationHandler<UpdateColorCommand>
    {
        private string Color { get; set; } = "#3a0647";
        private string SideBar => $"background-image: linear-gradient(180deg, rgb(5, 39, 103) 0%, {Color} 70%);";
        public static event EventHandler<UpdateColorEventArgs> ColorChanged;

        public async Task Handle(UpdateColorCommand notification, CancellationToken cancellationToken)
        {
            ColorChanged?.Invoke(this, new UpdateColorEventArgs { Color = notification.Color });
        }
        protected Task OnColorChanged(object obj, UpdateColorEventArgs args)
        {
            Color = args.Color;
            return InvokeAsync(StateHasChanged);
        }
    }
}
