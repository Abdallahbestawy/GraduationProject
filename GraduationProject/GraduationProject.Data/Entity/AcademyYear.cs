using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Data.Entity
{
    public class AcademyYear
    {
        [Key]
        public int Id { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string? Description { get; set; }
        public int AcademyYearOrder { get; set; }

        public bool IsCurrent { get; set; }
    }
}
