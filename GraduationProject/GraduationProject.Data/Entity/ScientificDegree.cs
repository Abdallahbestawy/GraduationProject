using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class ScientificDegree
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(1000)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ScientificDegreeType Type { get; set; }

        public int Order { get; set; }

        public decimal? SuccessPercentageBand { get; set; }

        public decimal? SuccessPercentageSemester { get; set; }

        public decimal? SuccessPercentagePhase { get; set; }

        [ForeignKey("Bylaw")]
        public int BylawId { get; set; }
        public Bylaw Bylaw { get; set; }

        [ForeignKey("Band")]
        public int? BandId { get; set; } = null;
        public Band? Band { get; set; }

        [ForeignKey("Phase")]
        public int? PhaseId { get; set; } = null;
        public Phase? Phase { get; set; }

        [ForeignKey("Semester")]
        public int? SemesterId { get; set; } = null;
        public Semester? Semester { get; set; }

        [ForeignKey("ExamRole")]
        public int? ExamRoleId { get; set; } = null;
        public ExamRole? ExamRole { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; } = null;
        public ScientificDegree? Parent { get; set; }

        public virtual ICollection<ScientificDegree> ParentScientificDegree { get; set; } = new List<ScientificDegree>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
    }
}
