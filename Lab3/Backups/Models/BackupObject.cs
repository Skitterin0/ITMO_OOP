using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models;

public class BackupObject : IObject, IEquatable<BackupObject>
{
    public BackupObject(string path, string objectName)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            BackupException.NullPath();
        }

        if (string.IsNullOrWhiteSpace(objectName))
        {
            BackupException.NullPath();
        }

        FullPath = Path.GetFullPath(path);
        ObjectName = objectName;
    }

    public string FullPath { get; }
    public string ObjectName { get; }

    public bool Equals(BackupObject? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FullPath == other.FullPath && ObjectName == other.ObjectName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((BackupObject)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FullPath, ObjectName);
    }
}