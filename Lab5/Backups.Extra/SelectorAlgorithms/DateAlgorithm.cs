using Backups.Entities;
using Backups.Extra.Tools;

namespace Backups.Extra.SelectorAlgorithms
{
    public class DateAlgorithm : ISelectorAlgorithm
    {
        private const int _zero = 0;
        private readonly TimeSpan _timeSpan;

        public DateAlgorithm(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
        }

        public List<RestorePoint> SelectPoints(List<RestorePoint> points)
        {
            if (points.Count == _zero)
            {
                BackupExtraException.NullInput(nameof(DateAlgorithm));
            }

            DateTime currTime = DateTime.Now;

            return points.Where(point => (point.CreationTime + _timeSpan) < currTime).ToList();
        }
    }
}