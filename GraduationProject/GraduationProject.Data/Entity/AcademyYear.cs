using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class AcademyYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string? Description { get; set; }
        public int AcademyYearOrder { get; set; }

        public bool IsCurrent { get; set; }
        public ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
        public virtual ICollection<StaffSemester> StaffSemesters { get; set; } = new List<StaffSemester>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}
