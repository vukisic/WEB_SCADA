using Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Owin.Security.Provider;
using System.Linq;
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

        public BasePointItem Single(int pointId)
        {
            BasePointItem item = DComCoreSingleton.GetSingleton().Points.SingleOrDefault(x => x.PointId == pointId);
            if (item != null)
                return item;
            return null;
        }
    }
}
