using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public Governorate Governorate { get; set; }

        public virtual ICollection<Staff> staff { get; set; } = new List<Staff>();
        public virtual ICollection<Student> students { get; set; } = new List<Student>();
        public virtual ICollection<FamilyData> familyDatas { get; set; } = new List<FamilyData>();
    }
}
