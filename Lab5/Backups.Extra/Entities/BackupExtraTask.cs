using System.Reflection;
using System.Text.Json.Serialization;
using Backups.Entities;
using Backups.Extra.CleanerAlgorithms;
using Backups.Extra.ExtraRepositories;
using Backups.Extra.Loggers;
using Backups.Extra.SelectorAlgorithms;
using Backups.Interfaces;

namespace Backups.Extra.Entities
{
    public class BackupExtraTask
    {
        // to change time open json and change creation time there
        public BackupExtraTask(IAlgorithm algorithm, ILoadRepository repository, ISelectorAlgorithm selector, ICleanerAlgorithm cleaner, ILogger logger)
        {
            Repository = new ExtraRepository(repository.FullPath);
            BaseTask = new BackupTask(algorithm, repository);
            (Selector, Cleaner, Logger) = (selector, cleaner, logger);
            Logger.LogMessage($"BackupExtraTask was created creation settings:\n{this}");
        }

        public IExtraRepository Repository { get; }
        public BackupTask BaseTask { get; }

        // returns list of points chosen for cleaning
        [JsonPropertyName("QuantityAlgorithm")]
        public ISelectorAlgorithm Selector { get; private set; }

        public ICleanerAlgorithm Cleaner { get; private set; }

        public ILogger Logger { get; private set; }

        public void SetAlgorithm(IAlgorithm newAlgorithm)
        {
            BaseTask.SetAlgorithm(newAlgorithm);
            Logger.LogMessage($"Backup algorithm changed to {newAlgorithm}");
        }

        public void AddBackupObject(IObject backupObject)
        {
            BaseTask.AddBackupObject(backupObject);
            Logger.LogMessage($"Added backup object {backupObject}");
        }

        public void DeleteBackupObject(IObject backupObject)
        {
            BaseTask.DeleteBackupObject(backupObject);
            Logger.LogMessage($"Removed backup object {backupObject}");
        }

        public void RemoveRestorePoint(RestorePoint point)
        {
            BaseTask.RemoveRestorePoint(point);
            Logger.LogMessage($"Removed restore point {point}");
        }

        public void CreateBackup()
        {
            BaseTask.CreateBackup();
            Logger.LogMessage($"New restore point was created: {BaseTask.Backup[^1]}");
            Logger.LogMessage("Backup was made");
        }

        public void SetSelector(ISelectorAlgorithm selector)
        {
            Selector = selector;
            Logger.LogMessage($"Restore points selector algorithm changed to {selector}");
        }

        public void SetCleaner(ICleanerAlgorithm cleaner)
        {
            Cleaner = cleaner;
            Logger.LogMessage($"Restore points cleaner algorithm changed to {cleaner}");
        }

        public void SetLogger(ILogger logger)
        {
            Logger.LogMessage($"The logger is about to be changed to {logger}. Bye!");
            Logger = logger;
            Logger.LogMessage($"Hello I'm your new logger");
        }

        public void ClearPoints()
        {
            var pointsToClear = new List<RestorePoint>(BaseTask.Backup);

            pointsToClear = Selector.SelectPoints(pointsToClear);

            if (BaseTask.Backup.Except(pointsToClear).Any())
            {
                pointsToClear.Remove(BaseTask.Backup[^1]);
            }

            Cleaner.ClearPoints(this, pointsToClear);
            Logger.LogMessage("Restore points were cleaned");
        }

        public override string ToString()
        {
            string returnString = $"Creation Backup methods:\n{BaseTask}\nSelector algorithm - {Selector}\n"
                                  + $"Cleaning algorithm - {Cleaner}\nLogging method - {Logger}\n";
            return returnString;
        }
    }
}