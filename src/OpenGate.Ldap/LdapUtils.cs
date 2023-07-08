using Novell.Directory.Ldap;

namespace OpenGate.Ldap
{
    public class LdapUtils
    {
        private readonly string _baseDn;
        private readonly bool _secure;
        private readonly HashSet<string> _servers;
        private string adminBindDn = string.Empty;
        private string adminBindpasswd = string.Empty;
        private LdapConnectionPool? _connectionPool;
        private LdapConnectionPool? _adminConnectionPool;

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

        public LdapConnectionPool CreateConnectionPool(string bindDn, string bindpasswd)
        {
            string[] hostPort = _servers.First().Split(":");
            LdapConnectionPool connectionPool = new LdapConnectionPool(hostPort[0], int.Parse(hostPort[1]), bindDn, bindpasswd);
            return connectionPool;
        }

        public LdapConnection GetConnectionFromPool()
        {

            if (_connectionPool == null)
            {
                _connectionPool = CreateConnectionPool(string.Empty, string.Empty);
            }

            return _connectionPool.GetConnection();
        }

        public LdapConnection GetAdminConnectionFromPool()
        {
            if (_adminConnectionPool == null)
            {
                _adminConnectionPool = CreateConnectionPool(adminBindDn, adminBindpasswd);
            }

            return _adminConnectionPool.GetConnection();
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
                _connectionPool?.ReleaseConnection(connection);
            }

            return authenticated;
        }

        public Dictionary<string, string> searchIdentityStoreConfiguration()
        {
            LdapConnection? connection = null;
            string searchBaseDn = $"{LdapUtilsConstants.IdentityStoreConnectionConfigDn},{this._baseDn}";
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

        public string AdminBindDn
        {
            get { return adminBindDn; }
            set { adminBindDn = value; }
        }

        public string AdminBindpasswd
        {
            get { return adminBindpasswd; }
            set { adminBindpasswd = value; }
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