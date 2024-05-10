using Microsoft.AspNetCore.SignalR;

namespace ConnectEduV2.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendChat()
        {
            await Clients.All.SendAsync("ReceiveChat");
        }
    }
}
