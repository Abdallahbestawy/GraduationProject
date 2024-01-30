using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        [Required, MaxLength(250)]
        public string Code { get; set; }

        public string? Description { get; set; }

        public CourseType Type { get; set; }

        public BylawType Category { get; set; }

        public decimal MaxDegree { get; set; }

        public int? NumberOfPoints { get; set; }

        public int? NumberOfCreditHours { get; set; }

        public bool Prerequisite { get; set; }

        [ForeignKey("ScientificDegree")]
        public int ScientificDegreeId { get; set; }
        public virtual ScientificDegree ScientificDegree { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<CoursePrerequisite> DependentCourses { get; set; } = new List<CoursePrerequisite>();
        public virtual ICollection<CourseAssessMethod> CourseAssessMethods { get; set; } = new List<CourseAssessMethod>();
        public virtual ICollection<StudentSemesterCourse> StudentSemesterCourse { get; set; } = new List<StudentSemesterCourse>();
    }
}
