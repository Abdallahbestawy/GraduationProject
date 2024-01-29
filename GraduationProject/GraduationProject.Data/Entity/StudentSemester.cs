using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class StudentSemester
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int DepartmentId { get; set; }

        public int ScientificDegreeId { get; set; }

        public int AcademyYear { get; set; }
    }
}
