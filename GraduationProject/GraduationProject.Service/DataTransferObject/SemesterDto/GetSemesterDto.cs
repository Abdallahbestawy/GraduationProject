namespace GraduationProject.Service.DataTransferObject.SemesterDto
{
    public class GetSemesterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Order { get; set; }

        public string FacultyName { get; set; }
    }
}
