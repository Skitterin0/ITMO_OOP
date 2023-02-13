namespace Backups.Exceptions
{
    public class BackupException : Exception
    {
        public BackupException(string message)
            : base(message)
        {
        }

        public BackupException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public static BackupException NullName(string name)
        {
            throw new BackupException($"The {name} name can't be empty");
        }

        public static BackupException AlreadyExists(string name, string collectionName)
        {
            throw new BackupException($"The {name} already exists in {collectionName} collection");
        }

        public static BackupException RemoveNull(string name, string collectionName)
        {
            throw new BackupException($"The {name} doesn't exists in {collectionName} collection");
        }

        public static BackupException NullPath()
        {
            throw new BackupException($"The file must have a path");
        }

        public static BackupException InvalidPath(string path)
        {
            throw new BackupException($"Was entered invalid path: {path}");
        }

        public static BackupException NoItemsToUpload()
        {
            throw new BackupException("Don't have any restore points to upload to repository");
        }

        public static BackupException NullReference(string objectName)
        {
            throw new BackupException($"{objectName} can't be null");
        }
    }
}