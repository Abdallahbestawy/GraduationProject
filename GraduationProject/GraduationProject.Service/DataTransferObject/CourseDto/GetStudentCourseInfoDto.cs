namespace GraduationProject.Service.DataTransferObject.CourseDto
{
    public class GetStudentCourseInfoDto
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public List<StudentCourseInfoDetialsDto> studentCourseInfoDetials { get; set; } = new List<StudentCourseInfoDetialsDto>();
    }
    public class StudentCourseInfoDetialsDto
    {
        public int StudentSemesterCourseId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string? Notes { get; set; }
    }
}
