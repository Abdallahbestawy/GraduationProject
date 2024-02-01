using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class CourseAssessMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [ForeignKey("AssessMethod")]
        public int AssessMethodId { get; set; }
        public AssessMethod AssessMethod { get; set; }

        public virtual ICollection<StudentSemesterAssessMethod> StudentSemesterAssessMethods { get; set; } = new List<StudentSemesterAssessMethod>();
    }
}
