using Backups.Entities;
using Backups.Extra.Tools;

namespace Backups.Extra.SelectorAlgorithms
{
    public class HybridAlgorithm : ISelectorAlgorithm
    {
        private const int _zero = 0;
        private readonly bool _useAll;
        private List<ISelectorAlgorithm> _algorithms;

        public HybridAlgorithm(List<ISelectorAlgorithm> algorithms, bool useAll)
        {
            if (algorithms.Count == _zero)
            {
                BackupExtraException.NullInput(nameof(HybridAlgorithm));
            }

            _algorithms = algorithms;
            _useAll = useAll;
        }

        public List<RestorePoint> SelectPoints(List<RestorePoint> points)
        {
            if (points.Count == _zero)
            {
                BackupExtraException.NullInput(nameof(HybridAlgorithm));
            }

            List<RestorePoint> listToReturn;
            if (_useAll)
            {
                listToReturn = new List<RestorePoint>(points);
            }
            else
            {
                listToReturn = new List<RestorePoint>();
            }

            foreach (ISelectorAlgorithm algorithm in _algorithms)
            {
                var listToDelete = algorithm.SelectPoints(points);
                if (_useAll)
                {
                    listToReturn = listToReturn.Intersect(listToDelete).ToList();
                }
                else
                {
                    listToReturn = listToReturn.Union(listToDelete).ToList();
                }
            }

            return listToReturn;
        }
    }
}