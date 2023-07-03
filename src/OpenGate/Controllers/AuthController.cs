using Microsoft.AspNetCore.Mvc;
using OpenGate.Models;
using Microsoft.Extensions.Configuration;

namespace OpenGate.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("opengate/rest/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost(Name = "auth")]
        public ActionResult<AuthenticationResponse> Login([FromBody] AuthenticationPayload authentication)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            bool result = LdapValidator.ValidateUserCredentials(configuration, authentication.username, authentication.password);
            return Ok(new {success = result});
        }
    }
}
