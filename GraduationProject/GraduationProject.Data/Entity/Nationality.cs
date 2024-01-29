using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class Nationality
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        //public List<Staff> Staff { get; set; }
    }
}
