using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Service.DataTransferObject.DepartmentDto
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int FacultyId { get; set; }
    }
}
