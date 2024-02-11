using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.ExamRolesDto
{
    public class ExamRolesDto
    {
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        [Required, MaxLength(250)]
        public string Code { get; set; }
        public int Order { get; set; }

        public int FacultyId { get; set; }
    }
}
