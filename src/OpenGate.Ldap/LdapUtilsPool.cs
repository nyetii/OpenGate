using Novell.Directory.Ldap;
using System;
using System.Collections.Concurrent;

namespace OpenGate.Ldap
{
    public class LdapConnectionPool
    {
        private readonly ConcurrentBag<LdapConnection> _connections;
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public LdapConnectionPool(string host, int port, string username, string password)
        {
            _connections = new ConcurrentBag<LdapConnection>();
            _host = host;
            _port = port;
            _username = username;
            _password = password;
        }

        public LdapConnection GetConnection()
        {
            if (_connections.TryTake(out var connection))
            {
                if (!connection.Connected)
                {
                    try
                    {
                        connection.Connect(_host, _port);
                        if (_username != string.Empty || _password != string.Empty)
                            connection.Bind(_username, _password);
                    }
                    catch (LdapException ex)
                    {
                        Console.WriteLine($"Failed to connect to LDAP server: {ex.Message}");
                        connection.Dispose();
                        connection = null;
                    }
                }

                if (connection != null)
                {
                    return connection;
                }
            }

            return CreateNewConnection();
        }

        public void ReleaseConnection(LdapConnection? connection)
        {
            if (connection is { Connected: true })
            {
                _connections.Add(connection);
            }
        }

        private LdapConnection CreateNewConnection()
        {
            var connection = new LdapConnection();
            try
            {
                connection.Connect(_host, _port);
                if (_username != string.Empty || _password != string.Empty)
                    connection.Bind(_username, _password);
                _connections.Add(connection);
            }
            catch (LdapException ex)
            {
                throw new Exception($"Failed to create a new LDAP connection: {ex.Message}");
            }

            return connection;
        }

        public void Dispose()
        {
            foreach (var connection in _connections)
            {
                connection.Dispose();
            }
        }
    }
}