using Backups.Extra.Tools;

namespace Backups.Extra.Loggers
{
    public class FileLogger : ILogger
    {
        private string logFile;

        public FileLogger(string path, bool isConfigurationEnabled)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                BackupExtraException.NullInput(nameof(FileLogger));
            }

            if (!File.Exists(path))
            {
                File.Create(path);
            }

            logFile = path;
            Configuration = new LogConfiguration(isConfigurationEnabled);
        }

        public LogConfiguration Configuration { get; }

        public void LogMessage(string message)
        {
            using var stream = new StreamWriter(logFile);
            stream.WriteLine(Configuration + message);
            stream.Close();
        }
    }
}