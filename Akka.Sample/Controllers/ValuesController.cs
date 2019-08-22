using Akka.Actor;
using Akka.Sample.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Akka.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IActorRef _logActor;

        public ValuesController(IActorFactory factory)
        {
            _logActor = factory.GetLogActor();
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logActor.Tell("Test");
            return new string[] { "value1", "value2" };
        }
    }
}
