namespace GraduationProject.Service.DataTransferObject.StudentDto
{
    public class GetStudentResultDto
    {
        public string StudentName { get; set; }

        public List<StudentResultDeltielsDto> StudentResultDeltiels { get; set; } = new List<StudentResultDeltielsDto>();

    }
    public class StudentResultDeltielsDto
    {
        public string SemesterName { get; set; }
        public string AcademyYearName { get; set; }
        public string BandName { get; set; }
        public string SemesterStatus { get; set; }
        public decimal? SemesterPercentage { get; set; }
        public char? SemesterChar { get; set; }
        public decimal? CumulativePercentage { get; set; }
        public char? CumulativeChar { get; set; }
        public List<StudentResultDeltielsSemesterDto> studentResultDeltielsSemester { get; set; } = new List<StudentResultDeltielsSemesterDto>();
    }
    public class StudentResultDeltielsSemesterDto
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int NumberOfPoint { get; set; }
        public string CourseDegree { get; set; }
        public string CourseChar { get; set; }
        public string CourseStatus { get; set; }
    }

}
