using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.Models;
using WebApi.Providers;

namespace WebApi.Controllers
{
    /// <summary>
    /// Application Main Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        #region Fields
        private IHubContext<AppHub> hub;
        #endregion
        public AppController(IHubContext<AppHub> hub)
        {
            this.hub = hub;
        }
        
        /// <summary>
        /// Handles GET Request for starting communication
        /// Route: /api/app
        /// </summary>
        /// <returns>Success of request</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var timerManager = new TimerManager(
                () => hub.Clients.All.SendAsync("recieveMsg", new 
                { 
                    status = DComCoreSingleton.GetSingleton().ConnectionState, 
                    list = DComCoreSingleton.GetSingleton().Points.ToList() 
                })
            );
            return Ok(new { Message = "Request Completed" });
        }

        /// <summary>
        /// Handles GET Request for log data
        /// Route: /api/app/logs
        /// </summary>
        /// <returns>Log data</returns>
        [HttpGet]
        [Route("Logs")]
        public IActionResult GetLogs()
        {
            return Ok(new { log = DComCoreSingleton.GetSingleton().GetLog() });
        }


        /// <summary>
        /// Handles POST request for commans
        /// Route: /api/app/command
        /// </summary>
        /// <param name="model">Model of a command from server</param>
        /// <returns>Success of command request</returns>
        [HttpPost]
        [Route("Command")]
        public IActionResult ExecuteCommand(CommandRequest model)
        {
            DComCoreSingleton.GetSingleton().ExecuteCommand(model.PointId, model.Address, model.Value);
            return Ok();
        }


    }
}