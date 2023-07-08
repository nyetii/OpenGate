namespace OpenGate.Ldap
{
    public class LdapUtilsConstants
    {
        public const string OpenGateConfigurationBaseDn = "ou=opengate-config,ou=internal";
        public const string IdentityStoreConnectionConfigDn = $"ou=config,ou=OpenGateLdapIdentityStorage,ou=services,{OpenGateConfigurationBaseDn}";
    }
}