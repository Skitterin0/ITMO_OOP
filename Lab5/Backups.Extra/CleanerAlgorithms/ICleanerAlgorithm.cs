using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.ExtraRepositories;

namespace Backups.Extra.CleanerAlgorithms
{
    public interface ICleanerAlgorithm
    {
        void ClearPoints(BackupExtraTask task, List<RestorePoint> pointsToClear);
    }
}