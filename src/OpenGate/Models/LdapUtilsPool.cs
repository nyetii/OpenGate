using Novell.Directory.Ldap;
using System;
using System.Collections.Concurrent;

namespace LdapUtils
{
    public class LdapConnectionPool
    {
        private ConcurrentBag<LdapConnection> connections;
        private string host;
        private int port;
        private string username;
        private string password;

        public LdapConnectionPool(string host, int port, string username, string password)
        {
            connections = new ConcurrentBag<LdapConnection>();
            this.host = host;
            this.port = port;
            this.username = username;
            this.password = password;
        }

        public LdapConnection GetConnection()
        {
            if (connections.TryTake(out LdapConnection connection))
            {
                if (!connection.Connected)
                {
                    try
                    {
                        connection.Connect(host, port);
                        connection.Bind(username, password);
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

        public void ReleaseConnection(LdapConnection connection)
        {
            if (connection != null && connection.Connected)
            {
                connections.Add(connection);
            }
        }

        private LdapConnection CreateNewConnection()
        {
            LdapConnection connection = new LdapConnection();
            try
            {
                connection.Connect(host, port);
                connection.Bind(username, password);
                connections.Add(connection);
            }
            catch (LdapException ex)
            {
                Console.WriteLine($"Failed to create a new LDAP connection: {ex.Message}");
                connection.Dispose();
                connection = null;
            }

            return connection;
        }

        public void Dispose()
        {
            foreach (LdapConnection connection in connections)
            {
                connection.Dispose();
            }
        }
    }
}