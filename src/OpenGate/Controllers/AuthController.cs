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

            user.Validate("nettypassword");

            if(user.IsValid)
                return Ok(user);

            return NotFound(user);
            
        }

        [HttpPost(Name = "PostAuth")]
        public ActionResult<bool> Login([FromBody] Authentication authentication)
        {
            var result = new User("opengate.io")
            {
                Name = authentication.username
            };

            result.Validate(authentication.password);

            if (result.IsValid) 
                return Ok(new {success = true});
            
            return BadRequest(new { success = false });
        }
    }
}
