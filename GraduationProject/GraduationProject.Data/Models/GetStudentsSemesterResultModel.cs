namespace GraduationProject.Data.Models
{
    public class GetStudentsSemesterResultModel
    {
        public string StudentCode { get; set; }
        public string SemesterName { get; set; }
        public string AcademyYear { get; set; }
        public string BandName { get; set; }
        public string StudentName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseDegree { get; set; }
        public string CourseChar { get; set; }
        public int NumberOfPoints { get; set; }
        public string Name { get; set; }
        public string Degree { get; set; }
        public string SemesterStatus { get; set; }
        public string CourseStatus { get; set; }
        public decimal? StudentSemesterPercentage { get; set; }
        public char? StudentSemesterChar { get; set; }
        public decimal? StudentCumulativePercentage { get; set; }
        public char? StudentCumulativeChar { get; set; }
    }
}
