using Backups.Entities;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Algorithms
{
    public class SplitStorage : IAlgorithm
    {
        public RestorePoint CreateRestorePoint(BackupTask task, int id)
        {
            var storages = new List<Storage>();

            foreach (IObject backupObject in task.Objects)
            {
                var storage = new Storage($"{backupObject.ObjectName}({id}).zip");

                storage.AddObject(backupObject);

                storages.Add(storage);
            }

            return new RestorePoint(storages, id);
        }
    }
}
