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

        public decimal? SuccessPercentageCourse { get; set; }

        public decimal? SuccessPercentageBand { get; set; }

        public decimal? SuccessPercentageSemester { get; set; }

        public decimal? SuccessPercentagePhase { get; set; }

        [ForeignKey("Bylaw")]
        public int? BylawId { get; set; }
        public virtual Bylaw Bylaw { get; set; }

        [ForeignKey("Band")]
        public int? BandId { get; set; }
        public virtual Band Band { get; set; }

        [ForeignKey("Phase")]
        public int? PhaseId { get; set; }
        public virtual Phase Phase { get; set; }

        [ForeignKey("Semester")]
        public int? SemesterId { get; set; }
        public virtual Semester Semester { get; set; }

        [ForeignKey("ExamRole")]
        public int? ExamRoleId { get; set; }
        public virtual ExamRole ExamRole { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public virtual ScientificDegree Parent { get; set; }

        public virtual ICollection<ScientificDegree> ParentScientificDegree { get; set; } = new List<ScientificDegree>();
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
        public virtual ICollection<Result> Results { get; set; } = new List<Result>();
    }
}
