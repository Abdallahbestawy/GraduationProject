using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Service.DataTransferObject.SchedulePlaceDto
{
    public class SchedulePlaceDto
    {
        public int Id { get; set; }
        [Required, MaxLength(550), MinLength(3)]
        public string Name { get; set; }
        public int PlaceCapacity { get; set; }
        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
    }
}
