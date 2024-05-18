using GraduationProject.Data.Enum;

namespace GraduationProject.Service.DataTransferObject.FormatStudentCodeDto
{
    public class FormatStudentCodeDto
    {
        public int? Id { get; set; }
        public FormatStudentCodeEnum FormatStudentCodeName { get; set; }
        public int FacultyId { get; set; }
    }
}
