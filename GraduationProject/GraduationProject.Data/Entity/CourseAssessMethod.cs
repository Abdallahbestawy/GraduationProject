using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class CourseAssessMethod
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("Course")]
        public int CourseId { get; set; }
        //public Course Course { get; set; }

        //[ForeignKey("AssessMethod")]
        public int AssessMethodId { get; set; }
        //public AssessMethod AssessMethod { get; set; }
    }
}
