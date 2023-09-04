using System;
using Microsoft.AspNetCore.SignalR;

namespace Core.Api.Hubs
{
    public class NotifyHub : Hub
    {
        public void Send(int type, int badge, string title, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("notify", type, badge, title, message);
        }
    }

    
}
