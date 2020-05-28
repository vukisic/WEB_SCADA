using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.Providers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private IHubContext<AppHub> hub;

        public AppController(IHubContext<AppHub> hub)
        {
            this.hub = hub;
        }

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

        [HttpGet]
        [Route("Logs")]
        public IActionResult GetLogs()
        {
            return Ok(new { log = DComCoreSingleton.GetSingleton().GetLog() });
        }
    }
}