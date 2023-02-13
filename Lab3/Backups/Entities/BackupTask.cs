using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Entities
{
    public class BackupTask
    {
        private int _id = 1;
        private IAlgorithm _algorithm;
        private List<RestorePoint> _backup;
        private List<IObject> _objects;

        public BackupTask(IAlgorithm algorithm, ILoadRepository repository)
        {
            _algorithm = algorithm;
            Repository = repository;
            _backup = new List<RestorePoint>();
            _objects = new List<IObject>();
        }

        public ILoadRepository Repository { get; }
        public IReadOnlyList<RestorePoint> Backup => _backup.AsReadOnly();
        public IReadOnlyList<IObject> Objects => _objects.AsReadOnly();

        public void SetAlgorithm(IAlgorithm newAlgorithm)
        {
            _algorithm = newAlgorithm;
        }

        public void AddBackupObject(IObject backupObject)
        {
            if (_objects.Contains(backupObject))
            {
                BackupException.AlreadyExists("BackupObject", "BackupTask objects");
            }

            _objects.Add(backupObject);
        }

        public void DeleteBackupObject(IObject backupObject)
        {
            if (!_objects.Contains(backupObject))
            {
                BackupException.RemoveNull("BackupObject", "BackupTask objects");
            }

            _objects.Remove(backupObject);
        }

        public void CreateBackup()
        {
            _backup.Add(_algorithm.CreateRestorePoint(this, GenerateId()));

            Repository.Upload(_backup[^1]);
        }

        protected int GenerateId()
        {
            return _id++;
        }
    }
}