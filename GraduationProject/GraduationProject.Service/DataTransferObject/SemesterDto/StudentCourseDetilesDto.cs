namespace GraduationProject.Service.DataTransferObject.SemesterDto
{
    public class StudentCourseDetilesDto
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseDegree { get; set; }
        public string CourseChar { get; set; }
        public int NumberOfPoints { get; set; }
        public string CourseStatus { get; set; }
        public List<CourseDegreeDetielsDto> CourseDegreeDetiles { get; set; } = new List<CourseDegreeDetielsDto>();
    }
    public class CourseDegreeDetielsDto
    {
        public string AssessMethodsName { get; set; }
        public string Degree { get; set; }
    }
}
