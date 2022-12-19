using UniversityDB.Models.DataModels;

namespace UniversityDB.Services
{
    public interface IStudentsService
    {
        IEnumerable<Student> GetStudentsWithCourses();
        IEnumerable<Student> GetStudentsWithNoCourses();

    }
}
