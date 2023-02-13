using Backups.Models;

namespace Backups.Entities
{
    public class RestorePoint : IEquatable<RestorePoint>
    {
        public RestorePoint(List<Storage> storages, int id)
        {
            Storages = storages.AsReadOnly();
            CreationTime = DateTime.Now;
            Name = $"RestorePoint({id})";
        }

        public IReadOnlyList<Storage> Storages { get; }
        public DateTime CreationTime { get; }
        public string Name { get; }
        public bool Equals(RestorePoint? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CreationTime.Equals(other.CreationTime) && Storages.Equals(other.Storages);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((RestorePoint)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CreationTime, Storages);
        }
    }
}