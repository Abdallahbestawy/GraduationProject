using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.ScientificDegreeDto
{
    public class ScientificDegreeDto
    {
        public int Id { get; set; }
        [Required, MaxLength(1000)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ScientificDegreeType Type { get; set; }

        public int Order { get; set; }


        public decimal? SuccessPercentageBand { get; set; }

        public decimal? SuccessPercentageSemester { get; set; }

        public decimal? SuccessPercentagePhase { get; set; }

        public int BylawId { get; set; }

        public int? BandId { get; set; }

        public int? PhaseId { get; set; }

        public int? SemesterId { get; set; }

        public int? ExamRoleId { get; set; }

        public int? ParentId { get; set; }
    }
}
