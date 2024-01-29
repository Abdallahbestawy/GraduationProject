using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class FamilyData
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("Student")]
        public int StudentId { get; set; }
        //public Student Student { get; set; }

        public string Name { get; set; }

        public string Job { get; set; }

        //[ForeignKey("Country")]
        public int CountryId { get; set; }
        //public Country Country { get; set; }

        //[ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        //public Governorate Governorate { get; set; }

        //[ForeignKey("City")]
        public int CityId { get; set; }
        //public City City { get; set; }

        public string? Street { get; set; }

        public string? PostalCode { get; set; }
    }
}
