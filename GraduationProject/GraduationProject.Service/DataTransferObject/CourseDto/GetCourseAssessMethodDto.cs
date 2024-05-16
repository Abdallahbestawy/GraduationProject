namespace GraduationProject.Service.DataTransferObject.CourseDto
{
    public class GetCourseAssessMethodDto
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public List<AssessMethodDetielsDto> AssessMethodDetiels { get; set; } = new List<AssessMethodDetielsDto>();
    }
    public class AssessMethodDetielsDto
    {
        public int AssessMethodId { get; set; }
        public string AssessMethodName { get; set; }

        public string? Description { get; set; }

        public decimal MinDegree { get; set; }

        public decimal MaxDegree { get; set; }
    }
}
