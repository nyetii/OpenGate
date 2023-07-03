using Microsoft.AspNetCore.Mvc;
using OpenGate.Models;

namespace OpenGate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet(Name = "GetTest")]
        public IActionResult Get()
        {
            var user = new User("opengate.io")
            {
                Name = "netty"
            };

            var isValid = user.Validate("nettypassword");

            if(isValid)
                return Ok(user);

            return NotFound(user);
            
        }
    }
}
