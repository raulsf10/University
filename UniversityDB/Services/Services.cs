using UniversityDB.DataAccess;
using UniversityDB.Models.DataModels;

namespace UniversityDB.Services
{
    public class Services
    {
        private readonly UniversityDBContext _context;

        public Services(UniversityDBContext context)
        {
            _context = context;
        }
        

        public User GetUserForEmail(string email)
        {
            return _context.Users.FirstOrDefault(user => user.Email.Equals(email));
             
        }

        public List<Student> GetStudentMoreAge()
        {
            return (List<Student>)_context.Students.Select(student => (DateTime.Now.AddTicks(-student.DoB.Ticks).Year - 1) >= 18);

        }



    }
}
