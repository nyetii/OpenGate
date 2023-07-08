using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGate.Ldap
{
    public class LdapManagerException : Exception
    {
        public LdapManagerException(string message) : base(message)
        {
        }
    }
}
