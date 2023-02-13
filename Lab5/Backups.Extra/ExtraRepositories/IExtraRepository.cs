using Backups.Entities;
using Backups.Interfaces;

namespace Backups.Extra.ExtraRepositories;

public interface IExtraRepository : ILoadRepository, IRepositoryTask
{
    string? RestorePath { get; set; }
    void DeleteRestorePoint(RestorePoint point);
    void MergeRestorePoints(RestorePoint mergedPoint, RestorePoint pointToMerge);
    void Restore(RestorePoint point);
}