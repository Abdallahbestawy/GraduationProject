namespace GraduationProject.Service.DataTransferObject.SemesterDto
{
    public class GetAllStudentInCourseResultDto
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int NumberOfPoints { get; set; }
        public List<CourseStudentCourseDetilesDto> CourseStudentCourseDetiles { get; set; } = new List<CourseStudentCourseDetilesDto>();

    }
    public class CourseStudentCourseDetilesDto
    {
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string CourseDegree { get; set; }
        public string CourseChar { get; set; }
        public string CourseStatus { get; set; }
        public List<CourseDegreeDetielsDto> CourseDegreeDetiles { get; set; } = new List<CourseDegreeDetielsDto>();

    }
}
