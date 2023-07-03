using Microsoft.AspNetCore.Mvc;
using OpenGate.Models;
using Microsoft.Extensions.Configuration;
using OpenGate.Logging;

namespace OpenGate.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("opengate/rest/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LdapValidator _ldapValidator;

        public AuthController(LdapValidator ldapValidator)
        {
            _ldapValidator = ldapValidator;
        }

        [HttpPost(Name = "auth")]
        public ActionResult<AuthenticationResponse> Login([FromBody] AuthenticationPayload authentication)
        {
            bool result = _ldapValidator.ValidateUserCredentials(authentication.username, authentication.password);
            return Ok(new {success = result});
        }
    }
}
