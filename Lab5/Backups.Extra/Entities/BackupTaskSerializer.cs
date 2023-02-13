using System.Text.Json;
using Backups.Extra.Loggers;
using Backups.Extra.Tools;

namespace Backups.Extra.Entities
{
    public class BackupTaskSerializer
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
        };

        private readonly string _jsonPath = string.Empty;

        public BackupTaskSerializer(ILogger logger)
        {
            Logger = logger;
            var directory = new DirectoryInfo(Environment.CurrentDirectory).Parent;
            if (directory != null)
            {
                _jsonPath = Path.Combine(directory.FullName, "task.json");
            }
            else
            {
                BackupExtraException.NullInput(nameof(BackupTaskSerializer));
            }
        }

        public ILogger Logger { get; }

        public void Serialize(BackupExtraTask task)
        {
            using var stream = new FileStream(_jsonPath, FileMode.Create);
            JsonSerializer.SerializeAsync(stream, task, _options);

            Logger.LogMessage("BackupExtraTask was saved");
        }

        public BackupExtraTask? Deserialize()
        {
            if (!File.Exists(_jsonPath))
            {
                BackupExtraException.NullJson();
            }

            using var stream = new FileStream(_jsonPath, FileMode.Open);

            Logger.LogMessage($"BackupExtraTask was loaded");
            return JsonSerializer.DeserializeAsync<BackupExtraTask>(stream, _options).Result;
        }
    }
}