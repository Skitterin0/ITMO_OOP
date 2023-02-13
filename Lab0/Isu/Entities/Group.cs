using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities
{
    public class Group
    {
        private const int _maxGroupSize = 23;
        private List<Student> _groupStudents;

        public Group(GroupName isuGroup, List<Student> isuStudents)
        {
            if (isuStudents.Count > _maxGroupSize)
            {
                throw new IsuException($"Group can't contain more than {_maxGroupSize} students");
            }

            GroupInfo = isuGroup;
            _groupStudents = isuStudents;
        }

        public Group(GroupName isuGroup)
            : this(isuGroup, new List<Student>()) { }

        public GroupName GroupInfo { get; }

        public int MaxGroupSize => _maxGroupSize;

        public IReadOnlyList<Student> GroupStudents => _groupStudents.AsReadOnly();

        public void AddStudent(Student newStudent)
        {
            if (IsStudentInGroup(newStudent))
            {
                throw new IsuException($"The student is already in the group - {newStudent.Name}");
            }

            if (GroupStudents.Count == _maxGroupSize)
            {
                throw new IsuException("The group is full, can't add new student");
            }

            _groupStudents.Add(newStudent);
        }

        public void RemoveStudent(Student isuStudent)
        {
            if (!IsStudentInGroup(isuStudent))
            {
                throw new IsuException($"The student isn't in the group - {isuStudent.Name}");
            }

            _groupStudents.Remove(isuStudent);
        }

        public bool IsStudentInGroup(Student isuStudent)
        {
            return GroupStudents.Contains(isuStudent);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Group group)
            {
                return group.GroupInfo.Value == GroupInfo.Value && group.GroupStudents == GroupStudents;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GroupInfo.Course, GroupInfo.GroupNumber, GroupInfo.Value);
        }
    }
}