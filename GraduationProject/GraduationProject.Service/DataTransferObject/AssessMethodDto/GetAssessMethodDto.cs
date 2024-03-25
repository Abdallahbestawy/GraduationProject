namespace GraduationProject.Service.DataTransferObject.AssessMethodDto
{
    public class GetAssessMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal MinDegree { get; set; }

        public decimal MaxDegree { get; set; }

        public string FacultyName { get; set; }
    }
}
