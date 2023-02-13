using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities
{
    public class Student
    {
        private string _name = string.Empty;
        public Student(string name, GroupName isuGroup, int id)
        {
            Name = name;
            IsuGroup = isuGroup;
            Id = id;
        }

        public string Name
        {
            get => _name;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new IsuException("The name can't be null");
                }

                _name = value;
            }
        }

        public int Id { get; }
        public GroupName IsuGroup { get; private set; }

        public void ChangeName(string newName)
        {
            Name = newName;
        }

        public void ChangeGroup(GroupName newGroup)
        {
            IsuGroup = newGroup;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Student student)
            {
                return student.Id == Id && student.Name == _name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}