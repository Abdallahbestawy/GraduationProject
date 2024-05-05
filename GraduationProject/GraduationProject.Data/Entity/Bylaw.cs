using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Bylaw
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(1000)]
        public string Name { get; set; }

        public string? Description { get; set; }
        public int GraduateValuerRequired { get; set; }

        public CourseCategory Type { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public virtual ICollection<Estimates> Estimatess { get; set; } = new List<Estimates>();
        public virtual ICollection<EstimatesCourse> EstimatesCourses { get; set; } = new List<EstimatesCourse>();
        public virtual ICollection<ScientificDegree> ScientificDegrees { get; set; } = new List<ScientificDegree>();
    }
}
