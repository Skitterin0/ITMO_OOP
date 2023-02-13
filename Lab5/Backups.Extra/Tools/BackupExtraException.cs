using Backups.Exceptions;

namespace Backups.Extra.Tools;

public class BackupExtraException : Exception
{
    public BackupExtraException(string message)
        : base(message) { }

    public static BackupExtraException NegativeValue(int value, string targetClass)
    {
        throw new BackupExtraException($"Was entered negative value({value}) in {targetClass}");
    }

    public static BackupExtraException NullInput(string targetClass)
    {
        throw new BackupExtraException($"Input of the null or empty container in {targetClass}");
    }

    public static BackupExtraException AlreadyExists(string obj, string collection)
    {
        throw new BackupExtraException($"{obj} alreadyExists in {collection}");
    }

    public static BackupExtraException NullRemoval(string obj, string collection)
    {
        throw new BackupExtraException($"{obj} doesn't exist in {collection}");
    }

    public static BackupExtraException NullJson()
    {
        throw new BackupExtraException($"Json with previous version of BackupTask doesn't exist");
    }

    public static BackupExtraException IncorrectJson(string obj)
    {
        throw new BackupExtraException($"Json doesn't contain the {obj}");
    }

    public static BackupExtraException NoRestorePointInRepository(string pointName, string repositoryPath)
    {
        throw new BackupExtraException($"The repository with path:{repositoryPath} doesn't contain point({pointName})");
    }

    public static BackupExtraException IncorrectPathInput(string path)
    {
        throw new BackupExtraException($"No such directory with following path: {path}");
    }

    public static BackupExtraException ConditionNotChanged(string condition)
    {
        throw new BackupException($"Log configuration is already {condition}");
    }
}