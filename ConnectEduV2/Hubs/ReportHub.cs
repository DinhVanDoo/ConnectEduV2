using Microsoft.AspNetCore.SignalR;

namespace ConnectEduV2.Hubs
{
    public class ReportHub : Hub
    {
        public async Task SendReportNotification()
        {
            await Clients.All.SendAsync("ReceiveReportNotification");
        }
    }
}
