using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class AssessMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal MinDegree { get; set; }

        public decimal MaxDegree { get; set; }

        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public virtual ICollection<CourseAssessMethod> CourseAssessMethods { get; set; } = new List<CourseAssessMethod>();
    }
}
