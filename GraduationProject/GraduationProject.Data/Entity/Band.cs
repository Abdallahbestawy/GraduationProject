using GraduationProject.Shared.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Band
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreLogging]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        [Required, MaxLength(250)]
        public string Code { get; set; }

        public int Order { get; set; }

        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        [IgnoreLogging]
        public Faculty Faculty { get; set; }

        [IgnoreLogging]
        public virtual ICollection<ScientificDegree> ScientificDegrees { get; set; } = new List<ScientificDegree>();
    }
}
