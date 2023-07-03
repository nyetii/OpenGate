using Microsoft.AspNetCore.Mvc;
using OpenGate.Models;

namespace OpenGate.Controllers.Test
{
    [ApiController]
    [Produces("application/json")]
    [Route("opengate/rest/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet(Name = "test")]
        public ActionResult Test()
        {
            return Ok(new {success = true});
        }
    }
}
