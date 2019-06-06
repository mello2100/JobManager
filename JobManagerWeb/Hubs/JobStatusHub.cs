using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace JobManagerWeb.Hubs
{
    public class JobStatusHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}