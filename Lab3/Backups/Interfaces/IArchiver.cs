using Backups.Models;

namespace Backups.Interfaces
{
    public interface IArchiver
    {
        void LoadArchive(string path, Storage storage);
    }
}