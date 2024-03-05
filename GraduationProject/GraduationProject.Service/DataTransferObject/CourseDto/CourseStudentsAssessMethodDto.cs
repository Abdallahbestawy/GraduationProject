using GraduationProject.Service.DataTransferObject.AssessMethodDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.DataTransferObject.CourseDto
{
    public class CourseStudentsAssessMethodDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set;}

        public List<AssessMethodWithDegree> AssessMethodWithDegrees { get; set; }
    }
}
