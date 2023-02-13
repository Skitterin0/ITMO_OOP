using Isu.Exceptions;

namespace Isu.Models
{
    public class CourseNumber
    {
        private const int _maxCourseNumber = 4;
        private const int _minCourseNumber = 1;

        public CourseNumber(int value)
        {
            if (value is < _minCourseNumber or > _maxCourseNumber)
            {
                throw new IsuException($"Was entered invalid course number - {value}");
            }

            Value = value;
        }

        public int Value { get; }
    }
}