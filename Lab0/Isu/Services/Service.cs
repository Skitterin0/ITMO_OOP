using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services
{
    public class Service : IIsuService
    {
        private List<Group> _groups;

        private IdGenerator _idGenerator = new IdGenerator();
        public Service()
        {
            _groups = new List<Group>();
        }

        public Group AddGroup(GroupName name)
        {
            var newGroup = new Group(name);

            if (FindGroup(name) != null)
            {
                throw new IsuException("The service already contains this group");
            }

            _groups.Add(newGroup);

            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            var newStudent = new Student(name, group.GroupInfo, _idGenerator.Generate());
            group.AddStudent(newStudent);

            return newStudent;
        }

        public Student GetStudent(int id)
        {
            return FindStudent(id) ??
                   throw new IsuException($"The student with this id wasn't found - {id}");
        }

        public Student? FindStudent(int id)
        {
            return _groups.SelectMany(group => group.GroupStudents).SingleOrDefault(student => student.Id == id);
        }

        public IReadOnlyList<Student> FindStudents(GroupName groupName)
        {
            return FindGroup(groupName)?.GroupStudents ?? new List<Student>().AsReadOnly();
        }

        public IReadOnlyList<Student> FindStudents(CourseNumber courseNumber)
        {
            return _groups.SelectMany(group => group.GroupStudents).Where(student => student.IsuGroup.Course == courseNumber).ToList().AsReadOnly();
        }

        public Group? FindGroup(GroupName groupName)
        {
            return _groups.SingleOrDefault(group => group.GroupInfo.Value == groupName.Value);
        }

        public IReadOnlyList<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.Where(group => group.GroupInfo.Course.Value == courseNumber.Value).ToList().AsReadOnly();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (student.IsuGroup == newGroup.GroupInfo)
            {
                throw new IsuException("Student can't transfer to the same group");
            }

            if (FindGroup(newGroup.GroupInfo) == null)
            {
                throw new IsuException($"The group doesn't exist in service - {newGroup.GroupInfo.Value}");
            }

            newGroup.AddStudent(student);

            Group studentGroup = _groups.Single(group => group.GroupInfo.Value == student.IsuGroup.Value);
            studentGroup.RemoveStudent(student);

            student.ChangeGroup(newGroup.GroupInfo);
        }

        private class IdGenerator
        {
            private int _id;

            public IdGenerator()
            {
                _id = 100000;
            }

            public int Generate()
            {
                return _id++;
            }
        }
    }
}