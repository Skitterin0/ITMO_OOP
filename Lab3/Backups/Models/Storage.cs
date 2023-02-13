using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models
{
    public class Storage
    {
        private List<IObject> _objects;

        public Storage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                BackupException.NullName("storage");
            }

            Name = name;
            _objects = new List<IObject>();
        }

        public string Name { get; }
        public IReadOnlyList<IObject> Objects => _objects.AsReadOnly();

        public void AddObject(IObject backupObject)
        {
            if (_objects.Contains(backupObject))
            {
                BackupException.AlreadyExists("backupObject", "storage objects");
            }

            _objects.Add(backupObject);
        }
    }
}