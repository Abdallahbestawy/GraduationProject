namespace GraduationProject.Service.DataTransferObject.ScientificDegreeDto
{
    public class GetDetailsByParentIdDto
    {
        public List<GetDetailsDtos>? GetDetailsDtos { get; set; } = new List<GetDetailsDtos>();
    }
    public class GetDetailsDtos
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
