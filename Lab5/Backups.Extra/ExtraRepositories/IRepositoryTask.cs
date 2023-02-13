using Backups.Entities;

namespace Backups.Extra.ExtraRepositories;

public interface IRepositoryTask
{
    void CleanRestorePoint(RestorePoint point);
}