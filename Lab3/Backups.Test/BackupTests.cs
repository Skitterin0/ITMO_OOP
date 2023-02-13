using Backups.Algorithms;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Models;
using Xunit;
using Xunit.Abstractions;

namespace Backups.Test
{
    public class BackupTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BackupTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CreateRestorePointsAndStorages()
        {
            var readRepository = new MockRepository();
            var archiver = new FileSystemArchiver(readRepository);
            var repository = new LocalRepository("..\\Repository");
            repository.SetArchiver(archiver);

            var task = new BackupTask(new SplitStorage(), repository);

            var directoryObject = new BackupObject("path\\to\\FileA.txt", "FileA");
            var fileObject = new BackupObject("path\\to\\FileB.txt", "FileB");
            task.AddBackupObject(fileObject);
            task.AddBackupObject(directoryObject);

            task.CreateBackup();
            task.DeleteBackupObject(directoryObject);
            task.CreateBackup();

            Assert.Equal(2, task.Backup.Count);
            Assert.Equal(3, task.Backup.SelectMany(point => point.Storages).ToList().Count);
        }

        [Fact]
        public void CreateBackupCreatesDirectoriesAndArchives()
        {
            string repositoryPath = Path.Combine("..\\Repository");

            var readRepository = new MockRepository();
            var archiver = new FileSystemArchiver(readRepository);
            var repository = new LocalRepository(repositoryPath);
            repository.SetArchiver(archiver);

            var task = new BackupTask(new SingleStorage(), repository);

            var directoryObject = new BackupObject("path\\to\\FileA.txt", "FileA");
            var fileObject = new BackupObject("path\\to\\FileB.txt", "FileB");
            task.AddBackupObject(fileObject);
            task.AddBackupObject(directoryObject);

            task.CreateBackup();

            Assert.True(Directory.Exists("..\\Repository"));
        }
    }
}