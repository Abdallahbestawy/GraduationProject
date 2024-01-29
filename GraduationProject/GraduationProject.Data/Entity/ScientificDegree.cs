using GraduationProject.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class ScientificDegree
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("Faculty")]
        public int? FacultyId { get; set; }
        //public Faculty Faculty { get; set; }

        //[ForeignKey("Bylaw")]
        public int? BylawId { get; set; }
        //public Bylaw Bylaw { get; set; }

        //[ForeignKey("Band")]
        public int? BandId { get; set; }
        //public Band Band { get; set; }

        //[ForeignKey("Phase")]
        public int? PhaseId { get; set; }
        //public Phase Phase { get; set; }

        //[ForeignKey("Semester")]
        public int? SemesterId { get; set; }
        //public Semester Semester { get; set; }

        //[ForeignKey("ExamRole")]
        public int? ExamRoleId { get; set; }
        //public ExamRole ExamRole { get; set; }

        //[ForeignKey("Parent")]
        public int ParentId { get; set; }
        //public ScientificDegree? Parent { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public ScientificDegreeType? Type { get; set; }

        public string? Order { get; set; }

        public decimal? SuccessPercentageCourse { get; set; }

        public decimal? SuccessPercentageBand { get; set; }

        public decimal? SuccessPercentageSemester { get; set; }

        public decimal? SuccessPercentagePhase { get; set; }


    }
}
