using Microsoft.AspNetCore.Mvc;
using OpenGate.Models;
using Microsoft.Extensions.Configuration;
using OpenGate.Logging;
using OpenGate.Ldap;

namespace OpenGate.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("opengate/rest/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILog _log;
        private readonly LdapService _ldap;
        public AuthController(LdapService ldap, ILog log)
        {
            _ldap = ldap;
            _log = log;
        }

        [HttpPost(Name = "auth")]
        public ActionResult<AuthenticationResponse> Login([FromBody] AuthenticationPayload authentication)
        {
            if (Request.Cookies["opengatesession"] != null)
            {
                //Request.Cookies.TryGetValue("opengatesession", out var value);
                var cookie = Request.Cookies["opengatesession"];

                return Ok(new { success = true });
            }

            try
            {
                var result = _ldap.Utils.Authenticate(authentication.username, authentication.password);
                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                return BadRequest(new { success = false });
            }
        }
    }
}
