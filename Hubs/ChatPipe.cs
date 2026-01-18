using Microsoft.AspNetCore.SignalR;
using Mobilis_Real_Time_Assistant.Service;

namespace Mobilis_Real_Time_Assistant.Hubs;

public class ChatPipe(ApiIntegration apiIntegration) : Hub
{
    private readonly ApiIntegration _apiIntegration = apiIntegration;

    public async Task SendMessage(string message)
    {
        // call ApiIntegration for Assistant response
        var response = await _apiIntegration.GetResponseAsync(message);

        // send back to all clients
        await Clients.All.SendAsync("ReceiveMessage", message, response);
    }
}