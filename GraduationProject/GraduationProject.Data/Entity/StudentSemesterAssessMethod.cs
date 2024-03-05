using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class StudentSemesterAssessMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("StudentSemester")]
        public int StudentSemesterId { get; set; }
        public StudentSemester StudentSemester { get; set; }

        [ForeignKey("CourseAssessMethod")]
        public int CourseAssessMethodId { get; set; }
        public CourseAssessMethod CourseAssessMethod { get; set; }

        public decimal? Degree { get; set; } = null;
    }
}
