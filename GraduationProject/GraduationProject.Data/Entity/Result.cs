using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class Result
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int DepartmentId { get; set; }

        public int ScientificDegreeId { get; set; }

        public int AcademyYear { get; set; }

        public decimal GPA { get; set; }
    }
}
