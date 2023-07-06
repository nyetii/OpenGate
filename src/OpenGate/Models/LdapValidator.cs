using Novell.Directory.Ldap;
using System.DirectoryServices.ActiveDirectory;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using OpenGate.Logging;

namespace OpenGate.Models
{
    public class LdapValidator
    {
        private readonly IConfiguration _configuration;
        private readonly ILog _log;

        public LdapValidator(IConfiguration configuration, ILog log)
        {
            _configuration = configuration;
            _log = log;
        }

        public bool ValidateUserCredentials(string username, string password)
        {
            try
            {
                string? ldapServer = _configuration["LdapConfiguration:Server"];
                string? ldapPort = _configuration["LdapConfiguration:Port"];
                string? baseDn = _configuration["LdapConfiguration:BaseDN"];
                bool.TryParse(_configuration["LdapConfiguration:UseSSL"], out bool useSSL);

                if (ldapServer is null || ldapPort is null || baseDn is null)
                {
                    throw new ApplicationException("LDAP configuration values are missing or null.");
                }

                using var connection = new LdapConnection { SecureSocketLayer = useSSL };
                connection.Connect(ldapServer, int.TryParse(ldapPort, out int parsedPort) ? parsedPort : 389);
                connection.Bind($"uid={username},ou=identities,{baseDn}", password);
                
                _log.Information(connection.Bound ? $"{username} successfully authenticated!" : $"{username} has not been authenticated.");
                return connection.Bound;
            }
            catch (LdapException ex)
            {
                _log.Error(ex.ToString());
                return false;
            }
        }
    }
}
