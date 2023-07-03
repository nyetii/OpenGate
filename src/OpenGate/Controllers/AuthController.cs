using Microsoft.AspNetCore.Mvc;
using OpenGate.Models;

namespace OpenGate.Controllers
{
    [ApiController]
    [Route("opengate/rest/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpGet(Name = "GetAuth")]
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
