using Microsoft.AspNetCore.SignalR;
using WebApi.Models;
using WebApi.Providers;

namespace WebApi.Hubs
{
    public class AppHub : Hub
    {
        public void Command(CommandRequest model)
        {
            DComCoreSingleton.GetSingleton().ExecuteCommand(model.PointId, model.Address, model.Value);
        }

        public string Logs()
        {
            return DComCoreSingleton.GetSingleton().GetLog();
        }
    }
}
