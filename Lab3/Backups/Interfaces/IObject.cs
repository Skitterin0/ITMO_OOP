namespace Backups.Interfaces
{
    public interface IObject
    {
        public string FullPath { get; }
        public string ObjectName { get; }
    }
}