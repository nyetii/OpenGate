﻿using Novell.Directory.Ldap;
using System.DirectoryServices.ActiveDirectory;
using System.Xml.Linq;

namespace OpenGate.Models
{
    public static class UserExtensions
    {
        public static bool Validate(this User user, string password)
        {
            try
            {
                using var connection = new LdapConnection { SecureSocketLayer = false };

                connection.Connect("openldap", user.Port);
                connection.Bind(user.DomainName, password);

                return connection.Bound;
            }
            catch (LdapException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public static string ToDomainName(this User user)
        {
            //$"uid={user.Name},ou=identities,dc={user.Domain},dc=io";
            var dn = $"uid={user.Name},ou=identities";
            dn += user.SubDomains.Aggregate("", (current, str) => current + $",dc={str}");
            
            return dn;
        }
    }
}
