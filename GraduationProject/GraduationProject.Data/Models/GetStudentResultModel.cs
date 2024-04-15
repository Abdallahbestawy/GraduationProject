namespace GraduationProject.Data.Models
{
    public class GetStudentResultModel
    {
        public string NameEnglish { get; set; }
        public string SemesterName { get; set; }
        public string AcademyYear { get; set; }
        public string BandName { get; set; }
        public string SemesterStatus { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int NumberOfPoints { get; set; }
        public string CourseDegree { get; set; }
        public string CourseChar { get; set; }
        public string CourseStatus { get; set; }
        public decimal? SemesterPercentage { get; set; }
        public char? SemesterChar { get; set; }
        public decimal? CumulativePercentage { get; set; }
        public char? CumulativeChar { get; set; }
    }
}
