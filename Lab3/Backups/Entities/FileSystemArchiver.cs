using System.IO.Compression;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Entities
{
    public class FileSystemArchiver : IArchiver
    {
        public FileSystemArchiver(IReadRepository repository)
        {
            Repository = repository;
        }

        public IReadRepository Repository { get; }

        public void LoadArchive(string path, Storage storage)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                BackupException.NullPath();
            }

            foreach (IObject obj in storage.Objects)
            {
                using var zipToOpen = new FileStream(Path.Combine(path, storage.Name), FileMode.Create);
                using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create);

                if (Directory.Exists(obj.FullPath))
                {
                    LoadDirectory(archive, obj.ObjectName, obj.FullPath);
                }
                else
                {
                    LoadFile(archive, Path.GetFileName(obj.FullPath), obj.FullPath);
                }
            }
        }

        private void LoadDirectory(ZipArchive archive, string archivePath, string path)
        {
            var directory = new DirectoryInfo(path);

            archive.CreateEntry($"{archivePath}\\");

            foreach (FileInfo file in directory.GetFiles())
            {
                LoadFile(archive, Path.Combine(archivePath, file.Name), file.FullName);
            }

            foreach (DirectoryInfo directoryInfo in directory.GetDirectories())
            {
                LoadDirectory(archive, Path.Combine(archivePath, directoryInfo.Name), directoryInfo.FullName);
            }
        }

        private void LoadFile(ZipArchive archive, string archivePath, string path)
        {
            byte[] fileBytes = Repository.Read(path);
            using var bytesStream = new MemoryStream(fileBytes);

            ZipArchiveEntry archiveEntry = archive.CreateEntry(archivePath);
            bytesStream.CopyTo(archiveEntry.Open());
        }
    }
}