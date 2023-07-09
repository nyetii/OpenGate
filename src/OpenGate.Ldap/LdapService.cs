using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGate.Logging;

namespace OpenGate.Ldap
{
    public class LdapService
    {
        public readonly HashSet<string> Servers;
        public LdapUtils Utils { get; set; }

        public bool IsSecure { get; }

        public string BaseDn { get; }
        public string ConfigurationBaseDn => "ou=opengate-config,ou=internal";
        public string IdentityStorage => $"ou=config,ou=OpenGateLdapIdentityStorage,ou=services,{ConfigurationBaseDn}";

        public LdapService(string host, int port, bool secure, string baseDn, ILog log) : this(new HashSet<string> { $"{host}:{port}" }, secure, baseDn, log)
        {
        }

        public LdapService(HashSet<string> servers, bool secure, string baseDn, ILog log)
        {
            Servers = servers;
            IsSecure = secure;
            BaseDn = baseDn;

            if (baseDn == string.Empty || baseDn.Length < 1)
            {
                throw new LdapManagerException("Invalid baseDN");
            }

            Utils = new LdapUtils(log, this);
        }
    }
}
