using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ViewModels;

namespace WebApi.Hubs
{
    public class AppHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("recieveMsg", new { status = Singleton.GetSingleton().main.ConnectionState, list = Singleton.GetSingleton().main.Points.ToList() });
        }
            
    }
}
