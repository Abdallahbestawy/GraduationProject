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
    public class Course
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("ScientificDegree")]
        public int? ScientificDegreeId { get; set; }
        //public ScientificDegree? ScientificDegree { get; set; }

        //[ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        //public Department Department { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public CourseType Type { get; set; }

        public BylawType Category { get; set; }

        public decimal MaxDegree { get; set; }

        public int? NumberOfPoints { get; set; }

        public int? NumberOfCreditHours { get; set; }

        public bool Prerequisite { get; set; }

        //public List<CoursePrerequisite> coursePrerequisites { get; set; }
    }
}
