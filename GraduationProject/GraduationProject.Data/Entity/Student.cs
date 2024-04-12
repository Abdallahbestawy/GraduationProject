using GraduationProject.Data.Enum;
using GraduationProject.Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

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

        public virtual ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
        public virtual FamilyData FamilyDatas { get; set; }
        public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
        public virtual QualificationData QualificationDatas { get; set; }
    }
}
