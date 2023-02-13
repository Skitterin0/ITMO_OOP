using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test
{
    public class IsuServiceTest
    {
        private readonly Service _isu = new Service();

        [Fact]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group m3206 = _isu.AddGroup(new GroupName("M3206"));
            Student student1 = _isu.AddStudent(m3206, "Kolya");
            Student student2 = _isu.AddStudent(m3206, "Liza");

            Assert.Contains(student1, m3206.GroupStudents);
            Assert.Contains(student2, m3206.GroupStudents);

            Assert.True(student1.IsuGroup.Value == m3206.GroupInfo.Value);
            Assert.True(student2.IsuGroup.Value == m3206.GroupInfo.Value);
        }

        [Fact]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Throws<IsuException>(() =>
            {
                Group m3206 = _isu.AddGroup(new GroupName("M3206"));

                for (int i = 0; i <= m3206.MaxGroupSize; ++i)
                {
                    _isu.AddStudent(m3206, "groupStudent");
                }
            });
        }

        [Fact]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Throws<IsuException>(() => _isu.AddGroup(new GroupName("M3hahaha")));
        }

        [Fact]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group m3206 = _isu.AddGroup(new GroupName("M3206"));
            Group m3205 = _isu.AddGroup(new GroupName("M3205"));
            Student student = _isu.AddStudent(m3206, "Vadim");

            _isu.ChangeStudentGroup(student, m3205);
            Assert.Contains(student, m3205.GroupStudents);
            Assert.DoesNotContain(student, m3206.GroupStudents);
        }
    }
}