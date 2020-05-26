using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private IHubContext<AppHub> _hub;

        public AppController(IHubContext<AppHub> hub)
        {
            _hub = hub;
        }

        public IActionResult Get()
        {
            var timerManager = new TimerManager(() => _hub.Clients.All.SendAsync("recieveMsg", new { status = Singleton.GetSingleton().main.ConnectionState, list = Singleton.GetSingleton().main.Points.ToList() }));
            return Ok(new { Message = "Request Completed" });
        }
    }
}