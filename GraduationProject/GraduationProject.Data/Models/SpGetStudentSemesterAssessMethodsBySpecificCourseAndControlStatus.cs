namespace GraduationProject.Data.Models
{
    public class SpGetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus
    {
        public int StudentSemesterAssessMethodsId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string AssessmentMethodName { get; set; }
        public decimal? Degree { get; set; }
        public bool IsControlStatus { get; set; }
    }
}
