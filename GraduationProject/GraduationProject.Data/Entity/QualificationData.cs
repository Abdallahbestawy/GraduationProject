using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class QualificationData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Student")]
        public int? StudentId { get; set; }
        public Student? Student { get; set; }

        [ForeignKey("Staff")]
        public int? StaffId { get; set; }
        public Staff? Staff { get; set; }

        public string? PreQualification { get; set; }

        public int? SeatNumber { get; set; }
        [DataType(DataType.Date)]

        public DateTime? QualificationYear { get; set; }

        public decimal? Degree { get; set; }



    }
}
