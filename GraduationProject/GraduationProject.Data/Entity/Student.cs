using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Student
    {
        [Key]
        int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public string PlaceOfBirth { get; set; }

        public Gender Gender { get; set; }

        public Nationality Nationality { get; set; }

        public Religion Religion { get; set; }

        public string? ReleasePlace { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public virtual Governorate Governorate { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        public string? Street { get; set; }

        public string? PostalCode { get; set; }

        public virtual ICollection<StudentSemester> StudentSemesters { get; set; } = new List<StudentSemester>();
        public virtual ICollection<Result> Results { get; set; } = new List<Result>();
        public virtual ICollection<FamilyData> familyDatas { get; set; } = new List<FamilyData>();
        public virtual ICollection<Phone> phones { get; set; } = new List<Phone>();
        public virtual ICollection<QualificationData> qualificationDatas { get; set; } = new List<QualificationData>();
    }
}
