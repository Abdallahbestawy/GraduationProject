using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        //public List<Bylaw> Bylaws { get; set; }

        //public List<ScientificDegree> scientificDegrees { get; set; }

        //public List<Department> Departments { get; set; }
    }
}
