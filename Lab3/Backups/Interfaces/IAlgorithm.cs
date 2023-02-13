using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IAlgorithm
    {
        RestorePoint CreateRestorePoint(BackupTask task, int id);
    }
}