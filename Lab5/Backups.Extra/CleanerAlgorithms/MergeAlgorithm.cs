using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.ExtraRepositories;
using Backups.Interfaces;

namespace Backups.Extra.CleanerAlgorithms
{
    public class MergeAlgorithm : ICleanerAlgorithm
    {
        private const int _zero = 0;
        private const int _one = 1;
        private const int _specialSize = 7; // (*).zip
        private const int _numberPose = 6;

        public void ClearPoints(BackupExtraTask task, List<RestorePoint> pointsToClear)
        {
            var mergeObjects = pointsToClear.SelectMany(point => point.Storages)
                .SelectMany(storage => storage.Objects).Distinct().ToList();

            var currentObjects = task.BaseTask.Objects;

            while (task.BaseTask.Objects.Count != _zero)
            {
                task.BaseTask.DeleteBackupObject(task.BaseTask.Objects[_zero]);
            }

            foreach (IObject mergeObject in mergeObjects)
            {
                task.BaseTask.AddBackupObject(mergeObject);
            }

            task.CreateBackup();
            task.Repository.CleanRestorePoint(task.BaseTask.Backup[^1]);

            while (task.BaseTask.Objects.Count != _zero)
            {
                task.BaseTask.DeleteBackupObject(task.BaseTask.Objects[_zero]);
            }

            foreach (IObject currentObject in currentObjects)
            {
                task.BaseTask.AddBackupObject(currentObject);
            }

            var newPoint = task.BaseTask.Backup[^1];
            foreach (RestorePoint point in pointsToClear)
            {
                task.Repository.MergeRestorePoints(newPoint, point);
                task.BaseTask.RemoveRestorePoint(point);
                task.Repository.DeleteRestorePoint(point);
            }
        }
    }
}