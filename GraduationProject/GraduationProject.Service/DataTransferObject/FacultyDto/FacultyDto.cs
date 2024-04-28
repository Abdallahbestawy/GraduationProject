using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.FacultyDto
{
    public class FacultyDto
    {
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
