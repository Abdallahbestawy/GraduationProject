namespace GraduationProject.Service.DataTransferObject.CourseDto
{
    public class UpdateCourseStudentsAssessMethodDto
    {
        public int StudentSemesterAssessMethodId { get; set; }
        public int CourseId { get; set; }
        public int AssessmentMethodId { get; set; }
        public decimal? Degree { get; set; }
    }
}
