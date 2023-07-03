using Novell.Directory.Ldap;
using System.DirectoryServices.ActiveDirectory;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;

namespace OpenGate.Models
{
    public static class LdapValidator
    {
        public static bool ValidateUserCredentials(IConfiguration configuration, string username, string password)
        {
            try
            {
                string? ldapServer = configuration["LdapConfiguration:Server"];
                string? ldapPort = configuration["LdapConfiguration:Port"];
                string? baseDn = configuration["LdapConfiguration:BaseDN"];
                bool.TryParse(configuration["LdapConfiguration:UseSSL"], out bool useSSL);

                if (ldapServer is null || ldapPort is null || baseDn is null)
                {
                    throw new ApplicationException("LDAP configuration values are missing or null.");
                }

                using var connection = new LdapConnection { SecureSocketLayer = useSSL };
                connection.Connect(ldapServer, int.TryParse(ldapPort, out int parsedPort) ? parsedPort : 389);
                connection.Bind($"uid={username},ou=identities,{baseDn}", password);
                return connection.Bound;
            }
            catch (LdapException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
