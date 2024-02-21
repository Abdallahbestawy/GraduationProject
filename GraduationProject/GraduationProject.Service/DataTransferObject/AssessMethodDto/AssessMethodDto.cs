using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.AssessMethodDto
{
    public class AssessMethodDto
    {
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal MinDegree { get; set; }

        public decimal MaxDegree { get; set; }

        public int FacultyId { get; set; }
    }
}
