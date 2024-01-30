using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class ExamRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        [Required, MaxLength(250)]
        public string Code { get; set; }
        public int Order { get; set; }

        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }

        public virtual ICollection<ScientificDegree> ScientificDegrees { get; set; } = new List<ScientificDegree>();
    }
}
