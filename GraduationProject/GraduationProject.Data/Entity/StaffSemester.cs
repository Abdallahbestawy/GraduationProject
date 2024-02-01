using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class StaffSemester
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        public Staff Staff { get; set; }

        [ForeignKey("AcademyYear")]
        public int AcademyYearId { get; set; }
        public AcademyYear AcademyYear { get; set; }
    }
}
