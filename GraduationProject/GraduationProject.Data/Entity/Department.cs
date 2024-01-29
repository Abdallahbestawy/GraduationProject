using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        //public Faculty Faculty { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
