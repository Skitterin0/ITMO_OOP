namespace Backups.Interfaces
{
    public interface IReadRepository
    {
        byte[] Read(string path);
    }
}