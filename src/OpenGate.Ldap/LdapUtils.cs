using Novell.Directory.Ldap;

namespace OpenGate.Ldap
{
    public class LdapUtils
    {
        private readonly string _baseDn;
        private readonly bool _secure;
        private readonly HashSet<string> _servers;

        public LdapUtils(string host, int port, bool secure, string baseDn) : this(new HashSet<string> { $"{host}:{port}" }, secure, baseDn)
        {
        }

        public LdapUtils(HashSet<string> servers, bool secure, string baseDn)
        {
            _servers = servers;
            _secure = secure;
            _baseDn = baseDn;

            if (baseDn == string.Empty || baseDn.Length < 1)
            {
                throw new LdapManagerException("Invalid baseDN");
            }
        }

        public LdapConnection GetConnectionFromPool()
        {
            var connection = new LdapConnection { SecureSocketLayer = _secure };
            string[] hostPort = _servers.First().Split(":");
            connection.Connect(hostPort[0], int.Parse(hostPort[1]));
            return connection;
        }

        public bool Authenticate(string userDn, string password)
        {
            LdapConnection? connection = null;
            bool authenticated = false;


            try
            {
                connection = GetConnectionFromPool();
                connection.Bind($"{userDn},{_baseDn}", password);
                authenticated = connection.Bound;
            }
            finally
            {
                connection?.Disconnect();
            }

            return authenticated;
        }
    }
}