using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace OpenGate.Logging
{
    public class LoggerManager : ILog
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public void Debug(string message) => _logger.Debug(message);

        public void Error(string message) => _logger.Error(message);

        public void Fatal(string message) => _logger.Fatal(message);

        public void Information(string message) => _logger.Info(message);

        public void Warning(string message) => _logger.Warn(message);
    }
}
