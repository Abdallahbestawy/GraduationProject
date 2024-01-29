using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        //public List<Governorate> Governorates { get; set; }
        //public List<Staff> Staff { get; set; }
        //public List<Student> Students { get; set; }
        //public List<FamilyData> FamilyDatas { get; set; }
    }
}
