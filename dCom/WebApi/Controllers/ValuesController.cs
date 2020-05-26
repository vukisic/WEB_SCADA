using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.WebCommunication;
using dCom.ViewModel;
using Microsoft.AspNetCore.Mvc;
using WebApi.Providers;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        Main main;
        public ValuesController()
        {
            main = Singleton.GetSingleton().main;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<BasePointItem>> Get()
        {
            
            return Ok(new { status = main.ConnectionState, list = main.Points.ToList() });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
