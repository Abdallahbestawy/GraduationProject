using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string PlaceOfBirth { get; set; }

        public Gender Gender { get; set; }

        public Nationality Nationality { get; set; }

        public Religion Religion { get; set; }

        public string? ReleasePlace { get; set; }
        [DataType(DataType.Date)]

        public DateTime? DateOfBirth { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public Governorate Governorate { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public City City { get; set; }

        public string? Street { get; set; }

        public string? PostalCode { get; set; }

        public virtual ICollection<Phone> phones { get; set; } = new List<Phone>();
        public virtual ICollection<StaffSemester> StaffSemesters { get; set; } = new List<StaffSemester>();
        public virtual ICollection<QualificationData> qualificationDatas { get; set; } = new List<QualificationData>();
    }
}
