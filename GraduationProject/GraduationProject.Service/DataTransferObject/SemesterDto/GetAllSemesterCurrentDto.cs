namespace GraduationProject.Service.DataTransferObject.SemesterDto
{
    public class GetAllSemesterCurrentDto
    {
        public string AcademyYearName { get; set; }
        public List<GetSemesterNameDto> semesterName { get; set; } = new List<GetSemesterNameDto>();
    }
}
