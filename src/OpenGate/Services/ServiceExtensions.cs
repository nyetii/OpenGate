using NLog;
using OpenGate.Ldap;
using OpenGate.Logging;
using ILog = OpenGate.Logging.ILog;

namespace OpenGate.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            LogManager.Setup()
                .LoadConfigurationFromFile($"{Environment.CurrentDirectory}/NLog.config", false);
            services.AddSingleton<ILog, LoggerManager>();
        }

        public static void ConfigureLdapService(this IServiceCollection services)
        {
            //services.AddSingleton<LdapManager>();
            services.AddSingleton<Ldap.LdapService>(x =>
            {
                var config = x.GetRequiredService<IConfiguration>();

                return new Ldap.LdapService(config["LdapConfiguration:Server"], int.Parse(config["LdapConfiguration:Port"]),
                    bool.Parse(config["LdapConfiguration:UseSSL"]), config["LdapConfiguration:BaseDN"], x.GetRequiredService<ILog>());
            });
        }
    }
}
