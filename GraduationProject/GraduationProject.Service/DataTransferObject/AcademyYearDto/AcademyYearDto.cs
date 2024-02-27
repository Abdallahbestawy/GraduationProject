using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.AcademyYearDto
{
    public class AcademyYearDto
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
        public string? Description { get; set; }
        public int AcademyYearOrder { get; set; }
        public int FacultyId { get; set; }

        public bool IsCurrent { get; set; }
    }
}
