using Backups.Entities;

namespace Backups.Interfaces
{
    public interface ILoadRepository
    {
        void Upload(RestorePoint point);
    }
}