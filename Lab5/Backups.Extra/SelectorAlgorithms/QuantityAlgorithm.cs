using Backups.Entities;
using Backups.Extra.Tools;

namespace Backups.Extra.SelectorAlgorithms
{
    public class QuantityAlgorithm : ISelectorAlgorithm
    {
        private const int _zero = 0;
        private readonly int _quantity;

        public QuantityAlgorithm(int quantity)
        {
            if (quantity <= _zero)
            {
                BackupExtraException.NegativeValue(quantity, nameof(QuantityAlgorithm));
            }

            _quantity = quantity;
        }

        public List<RestorePoint> SelectPoints(List<RestorePoint> points)
        {
            if (points.Count == _zero)
            {
                BackupExtraException.NullInput(nameof(QuantityAlgorithm));
            }

            if (points.Count < _quantity)
            {
                return new List<RestorePoint>();
            }

            return points.Take(points.Count - _quantity).ToList();
        }
    }
}