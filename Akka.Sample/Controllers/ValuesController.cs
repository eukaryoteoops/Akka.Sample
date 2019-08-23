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
        private IActorRef _demoActor;
        private IActorRef _deployActor;

        public ValuesController(IActorFactory factory)
        {
            _demoActor = factory.GetDemoActor();
            _deployActor = factory.GetDeployActor();

        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //_demoActor.Tell("Test");
            return new string[] { "value1", "value2" };
        }
    }
}
