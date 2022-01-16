using Microsoft.AspNetCore.SignalR;

namespace RealTimeBlazorWebassemblyApp.Server.Hubs;

public class RealTimeHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
