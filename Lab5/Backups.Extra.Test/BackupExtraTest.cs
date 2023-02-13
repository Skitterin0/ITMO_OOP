using Backups.Algorithms;
using Backups.Entities;
using Backups.Extra.CleanerAlgorithms;
using Backups.Extra.Entities;
using Backups.Extra.ExtraRepositories;
using Backups.Extra.Loggers;
using Backups.Extra.SelectorAlgorithms;
using Backups.Models;
using Xunit;
using Xunit.Abstractions;

namespace Backups.Extra.Test
{
    public class BackupExtraTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BackupExtraTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void DataSavesViaSerializer()
        {
            var readRepository = new MockRepository();
            var archiver = new FileSystemArchiver(readRepository);
            var localRepository = new ExtraRepository("..\\path");
            localRepository.SetArchiver(archiver);
            var logger = new ConsoleLogger(true);

            var serializer = new BackupTaskSerializer(logger);
            var task = new BackupExtraTask(
                new SplitStorage(),
                localRepository,
                new QuantityAlgorithm(5),
                new DeleteAlgorithm(),
                logger);

            task.AddBackupObject(new BackupObject("some\\path\\to\\file.txt", "file"));
            task.CreateBackup();
            serializer.Serialize(task);
        }

        [Fact]
        public void TaskCreatesRestorePointAndClearsThem()
        {
            var readRepository = new MockRepository();
            var archiver = new FileSystemArchiver(readRepository);
            var localRepository = new ExtraRepository("..\\path");
            localRepository.SetArchiver(archiver);
            var logger = new ConsoleLogger(true);

            var serializer = new BackupTaskSerializer(logger);
            var task = new BackupExtraTask(
                new SplitStorage(),
                localRepository,
                new QuantityAlgorithm(1),
                new DeleteAlgorithm(),
                logger);

            task.AddBackupObject(new BackupObject("some\\path\\to\\file.txt", "file"));
            task.AddBackupObject(new BackupObject("some\\path\\to\\file1.txt", "file1"));
            task.AddBackupObject(new BackupObject("some\\path\\to\\file2.txt", "file2"));
            task.CreateBackup();
            task.DeleteBackupObject(new BackupObject("some\\path\\to\\file2.txt", "file2"));
            task.CreateBackup();
            task.AddBackupObject(new BackupObject("some\\path\\to\\file2.txt", "file2"));
            task.CreateBackup();
            task.ClearPoints();

            Assert.Equal(1, task.BaseTask.Backup.Count);
        }

        [Fact]
        public void TaskCreatesRestorePointsAndMergesThem()
        {
            var readRepository = new MockRepository();
            var archiver = new FileSystemArchiver(readRepository);
            var localRepository = new ExtraRepository("..\\path");
            localRepository.SetArchiver(archiver);
            var logger = new ConsoleLogger(true);

            var serializer = new BackupTaskSerializer(logger);
            var task = new BackupExtraTask(
                new SplitStorage(),
                localRepository,
                new QuantityAlgorithm(1),
                new MergeAlgorithm(),
                logger);

            task.AddBackupObject(new BackupObject("some\\path\\to\\file.txt", "file"));
            task.AddBackupObject(new BackupObject("some\\path\\to\\file1.txt", "file1"));
            task.AddBackupObject(new BackupObject("some\\path\\to\\file2.txt", "file2"));
            task.CreateBackup();
            task.DeleteBackupObject(new BackupObject("some\\path\\to\\file2.txt", "file2"));
            task.CreateBackup();
            task.AddBackupObject(new BackupObject("some\\path\\to\\file2.txt", "file2"));
            task.CreateBackup();
            task.ClearPoints();

            Assert.Equal(2, task.BaseTask.Backup.Count);
        }

        [Fact]
        public void CreatesBackupAndRestoresIt()
        {
            var readRepository = new MockRepository();
            var archiver = new FileSystemArchiver(readRepository);
            var localRepository = new ExtraRepository("..\\path");
            localRepository.SetArchiver(archiver);
            var logger = new ConsoleLogger(true);

            var serializer = new BackupTaskSerializer(logger);
            var task = new BackupExtraTask(
                new SplitStorage(),
                localRepository,
                new QuantityAlgorithm(1),
                new MergeAlgorithm(),
                logger);

            task.AddBackupObject(new BackupObject("some\\path\\to\\file.txt", "file"));
            task.AddBackupObject(new BackupObject("some\\path\\to\\file1.txt", "file1"));
            task.AddBackupObject(new BackupObject("some\\path\\to\\file2.txt", "file2"));
            task.CreateBackup();

            localRepository.RestorePath = "..\\dir";
            localRepository.Restore(task.BaseTask.Backup[^1]);
        }
    }
}