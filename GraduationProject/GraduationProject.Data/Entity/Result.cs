using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Result
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("studentSemester")]
        public int StudentSemesterId { get; set; }
        public StudentSemester studentSemester { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? PercentageTotal { get; set; }
        public char? Char { get; set; }
        public char? CharTotal { get; set; }

    }
}
