using NLog;
using OpenGate.Logging;
using OpenGate.Models;
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
            services.AddSingleton<LdapValidator>();
        }
    }
}
