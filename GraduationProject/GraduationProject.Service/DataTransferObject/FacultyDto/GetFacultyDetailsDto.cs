namespace GraduationProject.Service.DataTransferObject.FacultyDto
{
    public class GetFacultyDetailsDto
    {
        public List<FacultyBylawDtos>? FacultyBylawDtos { get; set; } = new List<FacultyBylawDtos>();
        public List<FacultyDepatmentDtos>? FacultyDepatmentDtos { get; set; } = new List<FacultyDepatmentDtos>();
        public List<FacultyAssessMethodDtos>? FacultyAssessMethodDtos { get; set; } = new List<FacultyAssessMethodDtos>();

    }
    public class FacultyBylawDtos
    {
        public int Id { get; set; }
        public string BylawName { get; set; }
    }
    public class FacultyDepatmentDtos
    {
        public int Id { get; set; }
        public string DepatmentName { get; set; }
    }
    public class FacultyAssessMethodDtos
    {
        public int Id { get; set; }
        public string AssessMethodName { get; set; }
    }

}
