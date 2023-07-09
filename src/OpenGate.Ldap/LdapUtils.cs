using Novell.Directory.Ldap;
using OpenGate.Logging;

namespace OpenGate.Ldap
{
    public class LdapUtils
    {
        public LdapService Ldap { get; set; }

        private ILog _log;

        public string AdminBindDn { get; set; } = string.Empty;
        public string AdminBindpasswd { get; set; } = string.Empty;

        private LdapConnectionPool? _connectionPool;
        private LdapConnectionPool? _adminConnectionPool;
        
        public LdapUtils(ILog log, LdapService ldap)
        {
            Ldap = ldap;
            _log = log;
        }

        public LdapConnectionPool CreateConnectionPool(string bindDn, string bindpasswd)
        {
            string[] hostPort = Ldap.Servers.First().Split(":");
            LdapConnectionPool connectionPool = new LdapConnectionPool(hostPort[0], int.Parse(hostPort[1]), bindDn, bindpasswd);
            return connectionPool;
        }

        public LdapConnection GetConnectionFromPool(bool isAdmin = false)
        {
            //var bindDn = isAdmin ? adminBindDn : string.Empty;
            //var bindpasswd = isAdmin ? adminBindpasswd : string.Empty;

            (string bindDn, string bindpasswd) = isAdmin ? (AdminBindDn, AdminBindpasswd) : (string.Empty, string.Empty);
            
            _connectionPool ??= CreateConnectionPool(bindDn, bindpasswd);

            return _connectionPool.GetConnection();
        }

        public LdapConnection GetAdminConnectionFromPool()
        {
            _adminConnectionPool ??= CreateConnectionPool(AdminBindDn, AdminBindpasswd);

            return _adminConnectionPool.GetConnection();
        }

        public bool Authenticate(string userDn, string password)
        {
            LdapConnection? connection = null;
            bool authenticated;

            try
            {
                connection = GetConnectionFromPool();
                var dn = $"uid={userDn},ou=identities,{Ldap.BaseDn}";
                connection.Bind(dn, password);
                authenticated = connection.Bound;
            }
            catch (LdapException ex) when (ex.Message is "Invalid Credentials")
            {
                authenticated = false;
            }
            finally
            {
                _connectionPool?.ReleaseConnection(connection);
            }

            return authenticated;
        }

        public Dictionary<string, string> SearchIdentityStoreConfiguration()
        {
            LdapConnection? connection = null;
            string searchBaseDn = $"{Ldap.IdentityStorage},{Ldap.BaseDn}";
            string searchFilter = "(objectClass=svcfAttributableStore)";
            string[] attributes = { "svcfAttribute" };

            try
            {
                connection = GetAdminConnectionFromPool();

                var searchConstraints = new LdapSearchConstraints();
                searchConstraints.Dereference = LdapSearchConstraints.DerefAlways;

                var searchResults = connection.Search(
                    searchBaseDn,
                    LdapConnection.ScopeBase,
                    searchFilter,
                    attributes,
                    false,
                    searchConstraints
                );

                string[] svcfAttributes = searchResults.Next().GetAttribute("svcfAttribute").StringValueArray;
                return AttributeToDictionary(svcfAttributes);
            }
            finally
            {
                _adminConnectionPool?.ReleaseConnection(connection);
            }
        }
        
        public Dictionary<string, string> AttributeToDictionary(IEnumerable<string> input)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var item in input)
            {
                var split = item.Split('=').ToList();

                var key = split[0];

                split.RemoveAt(0);
                var value = string.Join('=', split);
                dictionary.Add(key, value);
            }

            return dictionary;
        }
    }
}