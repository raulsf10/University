using System.ComponentModel.DataAnnotations;

namespace UniversityDB.Models.DataModels
{
    public class Student : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public DateTime DoB { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
