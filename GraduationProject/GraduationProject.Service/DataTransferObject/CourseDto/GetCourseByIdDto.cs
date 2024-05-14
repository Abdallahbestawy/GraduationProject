using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.CourseDto
{
    public class GetCourseByIdDto
    {
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        [Required, MaxLength(250)]
        public string Code { get; set; }

        public string? Description { get; set; }

        public CourseType Type { get; set; }

        public CourseCategory Category { get; set; }

        public decimal MaxDegree { get; set; }
        public decimal MinDegree { get; set; }

        public int? NumberOfPoints { get; set; }

        public int? NumberOfCreditHours { get; set; }

        public bool Prerequisite { get; set; }

        public int ScientificDegreeId { get; set; }
        public int FacultyId { get; set; }
        public int DepartmentId { get; set; }
        public List<GetCoursePrerequisiteDto>? CoursePrerequisites { get; set; } = new List<GetCoursePrerequisiteDto>();
    }
    public class GetCoursePrerequisiteDto
    {
        public int CoursePrerequisiteId { get; set; }
        public string CoursePrerequisiteName { get; set; }
    }
}

