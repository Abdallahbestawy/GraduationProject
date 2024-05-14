using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.BylawDto
{
    public class GetBylawDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }
        public int GraduateValuerRequired { get; set; }

        public string Type { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        public string FacultyName { get; set; }
    }
}
