namespace GraduationProject.Service.DataTransferObject.CourseDto
{
    public class CourseAssessMethodDto
    {
        public List<CourseAssessMethodDtos> CourseAssessMethods { get; set; }
    }
    public class CourseAssessMethodDtos
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int AssessMethodsId { get; set; }
    }
}
