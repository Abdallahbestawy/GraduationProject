using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Estimates
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
        public decimal MaxGpa { get; set; }
        public decimal MinGpa { get; set; }

        [ForeignKey("Bylaw")]
        public int BylawId { get; set; }
        public virtual Bylaw Bylaw { get; set; }
    }
}
