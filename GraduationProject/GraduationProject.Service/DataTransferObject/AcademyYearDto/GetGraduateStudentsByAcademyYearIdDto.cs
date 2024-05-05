namespace GraduationProject.Service.DataTransferObject.AcademyYearDto
{
    public class GetGraduateStudentsByAcademyYearIdDto
    {
        public string AcademyYearName { get; set; }
        public List<GraduateStudentDetielsDto> GraduateStudentDetiels { get; set; } = new List<GraduateStudentDetielsDto>();


    }
    public class GraduateStudentDetielsDto
    {
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public decimal Percentage { get; set; }
        public char Char { get; set; }
    }
}
