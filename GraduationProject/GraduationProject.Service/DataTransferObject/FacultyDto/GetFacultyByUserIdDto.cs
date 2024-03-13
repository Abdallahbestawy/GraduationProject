namespace GraduationProject.Service.DataTransferObject.FacultyDto
{
    public class GetFacultyByUserIdDto
    {
        public List<GetFacultyDtos>? GetFacultyDtos { get; set; } = new List<GetFacultyDtos>();
    }
    public class GetFacultyDtos
    {
        public int FacultId { get; set; }
        public string FacultName { get; set; }
    }
}
