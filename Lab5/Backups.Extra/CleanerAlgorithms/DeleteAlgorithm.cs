using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.ExtraRepositories;

namespace Backups.Extra.CleanerAlgorithms;

public class DeleteAlgorithm : ICleanerAlgorithm
{
    public void ClearPoints(BackupExtraTask task, List<RestorePoint> pointsToClear)
    {
        foreach (RestorePoint point in pointsToClear)
        {
            task.BaseTask.RemoveRestorePoint(point);
        }

        foreach (RestorePoint point in pointsToClear)
        {
            task.Repository.DeleteRestorePoint(point);
        }
    }
}