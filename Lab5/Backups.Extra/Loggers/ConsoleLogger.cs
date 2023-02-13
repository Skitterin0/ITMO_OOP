namespace Backups.Extra.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger(bool isConfigeratioEnabled)
        {
            Configuration = new LogConfiguration(isConfigeratioEnabled);
        }

        public LogConfiguration Configuration { get; }

        public void LogMessage(string message)
        {
            Console.WriteLine(Configuration + message);
        }
    }
}