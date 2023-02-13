using Backups.Entities;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Algorithms
{
    public class SingleStorage : IAlgorithm
    {
        public RestorePoint CreateRestorePoint(BackupTask task, int id)
        {
            var storage = new Storage($"Archive({id}).zip");

            foreach (IObject backupObject in task.Objects)
            {
                storage.AddObject(backupObject);
            }

            return new RestorePoint(new List<Storage>() { storage }, id);
        }
    }
}