using Serilog;
using Logging.Interfaces;

namespace Logging.Services
{
    public class LoggerService : ILoggerService
    {
        public void LogInfo(string message)
        {
            Log.Information(message);
        }

        public void LogWarning(string message)
        {
            Log.Warning(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            if (ex != null)
                Log.Error(ex, message);
            else
                Log.Error(message);
        }
    }
}
