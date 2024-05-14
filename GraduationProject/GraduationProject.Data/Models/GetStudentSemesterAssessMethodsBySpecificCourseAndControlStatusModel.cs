namespace GraduationProject.Data.Models
{
    public class GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModel
    {
        public int StudentSemesterAssessMethodsId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int CourseId { get; set; }
        public string AssessmentMethodName { get; set; }
        public int AssessmentMethodId { get; set; }
        public decimal? Degree { get; set; }
        public bool IsControlStatus { get; set; }
    }
}
