using Backups.Entities;

namespace Backups.Extra.SelectorAlgorithms
{
    public interface ISelectorAlgorithm
    {
        List<RestorePoint> SelectPoints(List<RestorePoint> points);
    }
}