namespace GraduationProject.Data.Models
{
    public class GetStudentCourseInfoModel
    {
        public int StudentSemesterCourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string? Notes { get; set; }
    }
}
