namespace GraduationProject.Service.DataTransferObject.DepartmentDto
{
    public class GetDeDepartmentByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
    }
}
