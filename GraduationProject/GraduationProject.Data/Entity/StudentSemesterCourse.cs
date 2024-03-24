using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class StudentSemesterCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("StudentSemester")]
        public int StudentSemesterId { get; set; }
        public StudentSemester StudentSemester { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public decimal? CourseDegree { get; set; }
        public bool Passing { get; set; } = false;
        public char? Char { get; set; }
    }
}
