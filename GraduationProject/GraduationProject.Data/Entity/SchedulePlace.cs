using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class SchedulePlace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(550), MinLength(3)]
        public string Name { get; set; }
        public int PlaceCapacity { get; set; }
        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
