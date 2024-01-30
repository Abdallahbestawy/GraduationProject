using GraduationProject.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class Phone
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Student")]
        public int? StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        public virtual Staff Staff { get; set; }

        public string PhoneNumber { get; set; }

        public PhoneType Type { get; set; }
    }
}
