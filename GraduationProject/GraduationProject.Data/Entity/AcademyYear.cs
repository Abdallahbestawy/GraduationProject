using GraduationProject.Shared.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class AcademyYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreLogging]
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

        [IgnoreLogging]
        public virtual Faculty Facultys { get; set; }
        [IgnoreLogging]
        public virtual ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
        [IgnoreLogging]
        public virtual ICollection<StaffSemester> StaffSemesters { get; set; } = new List<StaffSemester>();
    }
}
