using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Entities
{
    public class LocalRepository : IReadRepository, ILoadRepository
    {
        private IArchiver? _archiver;
        private DirectoryInfo _path;

        public LocalRepository(string path)
        {
            _path = new DirectoryInfo(path);

            if (!_path.Exists)
            {
                Directory.CreateDirectory(path);
            }
        }

        public string FullPath => _path.FullName;

        public void SetArchiver(IArchiver archiver)
        {
            _archiver = archiver;
        }

        public byte[] Read(string path)
        {
            return File.ReadAllBytes(path);
        }

        public void Upload(RestorePoint point)
        {
            DirectoryInfo newDirectory = Directory.CreateDirectory(Path.Combine(FullPath, point.Name));

            if (_archiver is null)
            {
                throw BackupException.NullReference("Archiver");
            }

            foreach (Storage storage in point.Storages)
            {
                _archiver.LoadArchive(newDirectory.FullName, storage);
            }
        }
    }
}
