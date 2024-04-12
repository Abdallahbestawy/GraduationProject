using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class AcademyYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        public string? Description { get; set; }
        public int AcademyYearOrder { get; set; }

        public bool IsCurrent { get; set; }
        [ForeignKey("Facultys")]
        public int FacultyId { get; set; }
        public virtual Faculty Facultys { get; set; }
        public virtual ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
        public virtual ICollection<StaffSemester> StaffSemesters { get; set; } = new List<StaffSemester>();
    }
}
