using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class EstimatesCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        [Required]
        public char Char { get; set; }
        public decimal MaxPercentage { get; set; }
        public decimal MinPercentage { get; set; }

        [ForeignKey("Bylaw")]
        public int BylawId { get; set; }
        public Bylaw Bylaw { get; set; }
    }
}
