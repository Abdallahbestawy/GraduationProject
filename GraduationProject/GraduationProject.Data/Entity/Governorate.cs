using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class Governorate
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        //[ForeignKey("Country")]
        public int CountryId { get; set; }
        //public Country Country { get; set; }

        //public List<City> cities { get; set; }
        //public List<Staff> Staff { get; set; }
        //public List<Student> Students { get; set; }
        //public List<FamilyData> FamilyDatas { get; set; }
    }
}
