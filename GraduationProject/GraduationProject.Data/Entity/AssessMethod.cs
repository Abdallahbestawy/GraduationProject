using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class AssessMethod
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        //public Faculty Faculty { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal MinDegree { get; set; }

        public decimal MaxDegree { get; set; }

    }
}
