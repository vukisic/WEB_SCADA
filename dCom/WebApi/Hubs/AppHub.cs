using Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Owin.Security.Provider;
using System.Linq;
using WebApi.Models;
using WebApi.Providers;

namespace WebApi.Hubs
{
    /// <summary>
    /// Application Main SingalR Hub
    /// </summary>
    public class AppHub : Hub
    {
        /// <summary>
        /// (SignalR - HTTP POST)
        /// Metod that handles command request througt client hub connection
        /// </summary>
        /// <param name="model">Model of an command from client </param>
        public void Command(CommandRequest model)
        {
            DComCoreSingleton.GetSingleton().ExecuteCommand(model.PointId, model.Address, model.Value);
        }

        /// <summary>
        /// (SignalR - HTTP GET)
        /// Metod that handles request for log data througt client hub connection
        /// </summary>
        /// <returns>Log data</returns>
        public string Logs()
        {
            return DComCoreSingleton.GetSingleton().GetLog();
        }

        /// <summary>
        /// (SignalR - HTTP POST)
        /// Metod that handles request for reading data througt client hub connection
        /// </summary>
        /// <param name="pointId">Id of point to be readed</param>
        /// <returns>Point if exists</returns>
        public BasePointItem Single(int pointId)
        {
            BasePointItem item = DComCoreSingleton.GetSingleton().Points.SingleOrDefault(x => x.PointId == pointId);
            if (item != null)
                return item;
            return null;
        }
    }
}
