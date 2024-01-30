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

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public virtual ICollection<City> cities { get; set; } = new List<City>();
        public virtual ICollection<Staff> staff { get; set; } = new List<Staff>();
        public virtual ICollection<Student> students { get; set; } = new List<Student>();
        public virtual ICollection<FamilyData> familyDatas { get; set; } = new List<FamilyData>();
    }
}
