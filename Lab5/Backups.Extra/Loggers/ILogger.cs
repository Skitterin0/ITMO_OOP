namespace Backups.Extra.Loggers
{
    public interface ILogger
    {
        LogConfiguration Configuration { get; }
        void LogMessage(string message);
    }
}