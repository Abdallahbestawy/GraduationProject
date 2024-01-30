using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}
