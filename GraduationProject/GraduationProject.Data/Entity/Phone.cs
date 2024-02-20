using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Phone
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
        [Required, MaxLength(11)]
        [RegularExpression(@"^\d+$")]
        public string PhoneNumber { get; set; }

        public PhoneType Type { get; set; }
    }
}
