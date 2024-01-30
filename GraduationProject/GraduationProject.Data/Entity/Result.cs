using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Result
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [ForeignKey("ScientificDegree")]
        public int ScientificDegreeId { get; set; }
        public virtual ScientificDegree ScientificDegree { get; set; }

        [ForeignKey("AcademyYear")]
        public int AcademyYearId { get; set; }
        public virtual AcademyYear AcademyYear { get; set; }

        public decimal GPA { get; set; }
    }
}
