namespace GraduationProject.Service.DataTransferObject.SemesterDto
{
    public class GetStudentsSemesterResultDto
    {
        public string SemesterName { get; set; }
        public string AcademyYearName { get; set; }
        public List<StudentsDetielsDto> studentsDetiels { get; set; } = new List<StudentsDetielsDto>();
    }
    public class StudentsDetielsDto
    {
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public decimal? StudentSemesterPercentage { get; set; }
        public char? StudentSemesterChar { get; set; }
        public decimal? StudentCumulativePercentage { get; set; }
        public char? StudentCumulativeChar { get; set; }
        public string StudentSemesterStatus { get; set; }
        public List<StudentCourseDetilesDto> StudentCourseDetiles { get; set; } = new List<StudentCourseDetilesDto>();
    }

}
