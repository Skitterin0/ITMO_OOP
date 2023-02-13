using System.IO.Compression;
using Backups.Entities;
using Backups.Extra.Tools;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Extra.ExtraRepositories
{
    public class ExtraRepository : LocalRepository, IExtraRepository, IRepositoryTask
    {
        private const int _zero = 0;
        private const int _one = 1;
        private const int _specialSize = 7;
        private const int _numberEndPose = 6;
        public ExtraRepository(string path)
            : base(path)
        {
        }

        public string? RestorePath { get; set; }

        public void CleanRestorePoint(RestorePoint point)
        {
            var directory = new DirectoryInfo(Path.Combine(FullPath, point.Name));
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
        }

        public void DeleteRestorePoint(RestorePoint point)
        {
            var repository = new DirectoryInfo(FullPath);
            if (repository.GetDirectories().SingleOrDefault(directory => directory.Name == point.Name) != null)
            {
                Directory.Delete(Path.Combine(repository.FullName, point.Name), true);
            }
        }

        public void MergeRestorePoints(RestorePoint mergedPoint, RestorePoint pointToMerge)
        {
            if (pointToMerge.Storages[_zero].Objects.Count != _one ||
                (pointToMerge.Storages.Count == _one && pointToMerge.Storages[0].Name.Contains("Archive")))
            {
                return;
            }

            var mergeDirectory = new DirectoryInfo(Path.Combine(FullPath, mergedPoint.Name));
            var directoryToMerge = new DirectoryInfo(Path.Combine(FullPath, pointToMerge.Name));

            foreach (FileInfo fileInfo in directoryToMerge.GetFiles())
            {
                FileInfo? matchName = mergeDirectory.GetFiles().SingleOrDefault(file
                    => file.Name.Substring(_zero, file.Name.Length - _specialSize) ==
                       fileInfo.Name.Substring(_zero, fileInfo.Name.Length - _specialSize));

                if (matchName != null)
                {
                    if (matchName.Name[^_numberEndPose] < fileInfo.Name[^_numberEndPose])
                    {
                        matchName.Delete();
                        fileInfo.CopyTo(Path.Combine(mergeDirectory.FullName, fileInfo.Name));
                    }
                }
                else
                {
                    fileInfo.CopyTo(Path.Combine(mergeDirectory.FullName, fileInfo.Name));
                }
            }
        }

        public void Restore(RestorePoint point)
        {
            var repositoryDirectory = new DirectoryInfo(FullPath);
            DirectoryInfo pointDirectory = repositoryDirectory.GetDirectories()
                .Single(directory => directory.Name == point.Name);

            foreach (FileInfo file in pointDirectory.GetFiles())
            {
                Storage currStorage = point.Storages.Single(storage => storage.Name == file.Name);

                using ZipArchive archive = ZipFile.OpenRead(file.FullName);
                var directoryObjects = new List<IObject>();
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    IObject obj = currStorage.Objects
                        .Single(obj => Path.GetFileName(obj.FullPath) == entry.FullName.Split(Path.PathSeparator)[_zero]);

                    string restorePath;
                    if (RestorePath != null)
                    {
                        restorePath = Path.Combine(RestorePath, entry.FullName);
                    }
                    else
                    {
                        string tmpPath = entry.FullName;
                        restorePath = Path.Combine(obj.FullPath, tmpPath[(tmpPath.IndexOf(Path.PathSeparator) + 1) ..]);
                    }

                    string? directoryPath = Path.GetDirectoryName(restorePath);

                    if (directoryPath == null)
                    {
                        throw new BackupExtraException("No such path");
                    }

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    if (directoryPath == restorePath)
                    {
                        Directory.Delete(directoryPath);
                        Directory.CreateDirectory(directoryPath);
                    }
                    else if (File.Exists(restorePath))
                    {
                        File.Delete(restorePath);
                        entry.ExtractToFile(restorePath);
                    }
                    else
                    {
                        entry.ExtractToFile(restorePath);
                    }
                }
            }
        }
    }
}