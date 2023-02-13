using Isu.Exceptions;

namespace Isu.Models
{
    public class GroupName
    {
        private const int _groupNameLength = 5;

        public GroupName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new IsuException("The group must have a name");
            }

            if (name.Length != _groupNameLength)
            {
                throw new IsuException($"The group's name has an invalid length - {name}");
            }

            if (!char.IsLetter(name[0]))
            {
                throw new IsuException($"The faculty must contain a letter - {name[0]}");
            }

            if (!char.IsDigit(name[1]))
            {
                throw new IsuException($"The faculty must contain a digit - {name[1]}");
            }

            if (!char.IsDigit(name[2]))
            {
                throw new IsuException($"The course value must be a number - {name[2]}");
            }

            if (!char.IsDigit(name[3]) || !char.IsDigit(name[4]))
            {
                throw new IsuException($"Was entered invalid group number - {name.Substring(2, 2)}");
            }

            Value = name;

            Course = new CourseNumber(int.Parse(name.Substring(2, 1)));

            GroupNumber = int.Parse(name.Substring(3, 2));
        }

        public CourseNumber Course { get; }
        public int GroupNumber { get; }
        public string Value { get; }
    }
}