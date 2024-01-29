using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class StudentSemesterAssessMethod
    {
        public int Id { get; set; }

        public int StudentSemesterId { get; set; }

        public int CourseAssessMethod { get; set; }

        public decimal CourseDegree { get; set; }
    }
}
