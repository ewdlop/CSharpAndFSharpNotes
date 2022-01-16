using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace RealTimeBlazorWebassemblyApp.Client.Pages
{
    public partial class Index : IAsyncDisposable
    {
        [Inject]
        private NavigationManager? NavigationManager { get; init; }
        private HubConnection? _hubConnection;
        private List<string> _messages = new List<string>();
        private string? _userInput;
        private string? _messageInput;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(NavigationManager.ToAbsoluteUri("/realtimehub")).Build();
            _hubConnection.On<string, string>("ReceiveMessages", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                _messages.Add(encodedMsg);
                StateHasChanged();
            });
            _hubConnection.On<string, string[]>("ReceiveMessages", (user, messages) =>
            {
                _messages = _messages.Union(messages).ToList();
                StateHasChanged();
            });
            await _hubConnection.StartAsync();
        }

        private async Task Send()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.SendAsync("SendMessage", _userInput, _messageInput);
            }
        }

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}