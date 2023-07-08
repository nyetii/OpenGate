using Novell.Directory.Ldap;

namespace LdapUtils
{
    public class LdapManager
    {
        private string baseDn;
        private bool secure;
        private HashSet<String> servers;

        public LdapManager(string host, int port, bool secure, string baseDn) : this(new HashSet<String> { $"{host}:{port}" }, secure, baseDn)
        {
        }

        public LdapManager(HashSet<String> servers, bool secure, string baseDn)
        {
            this.servers = servers;
            this.secure = secure;
            this.baseDn = baseDn;

            if (baseDn == string.Empty || baseDn.Length < 1)
            {
                throw new LdapManagerException("Invalid baseDN");
            }
        }

        public LdapConnection GetConnectionFromPool()
        {
            LdapConnection connection = new LdapConnection { SecureSocketLayer = secure };
            string[] host_port = servers.First().Split(":");
            connection.Connect(host_port[0], int.Parse(host_port[1]));
            return connection;
        }

        public bool Authenticate(string userDn, string password)
        {
            LdapConnection? connection = null;
            bool authenticated = false;


            try
            {
                connection = GetConnectionFromPool();
                connection.Bind($"{userDn},{baseDn}", password);
                authenticated = connection.Bound;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Disconnect();
                }
            }

            return authenticated;
        }
    }
}