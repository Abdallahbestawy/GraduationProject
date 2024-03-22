using GraduationProject.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.DataTransferObject.StudentDto
{
    public class SemesterStudentsDTO
    {
        public int ScientificDegreeId { get; set; }
        public List<StudentSemester> Students { get; set; }
    }
}
