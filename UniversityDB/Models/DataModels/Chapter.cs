using System.ComponentModel.DataAnnotations;

namespace UniversityDB.Models.DataModels
{
    public class Chapter : BaseEntity
    {
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = new Course();

        [Required]
        public string List { get; set; } = string.Empty;
    }
}
