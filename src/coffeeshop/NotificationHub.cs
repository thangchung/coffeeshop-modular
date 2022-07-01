using Microsoft.AspNetCore.SignalR;

namespace CoffeeShop;

public interface INotificationClient
{
    Task SendMessage(string message);
}

public class NotificationHub : Hub<INotificationClient>
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendMessage(message);
    }
}
