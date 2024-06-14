using GraduationProject.Data.Enum;
using GraduationProject.Identity.Models;
using GraduationProject.Shared.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GraduationProject.Data.Entity
{
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreLogging]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [IgnoreLogging]
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
        [IgnoreLogging]
        public Country Country { get; set; }

        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        [IgnoreLogging]
        public Governorate Governorate { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        [IgnoreLogging]
        public City City { get; set; }

        public string? Street { get; set; }

        public string? PostalCode { get; set; }
        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        [IgnoreLogging]
        public Faculty Faculty { get; set; }

        [IgnoreLogging]
        public virtual ICollection<Phone> phones { get; set; } = new List<Phone>();
        [IgnoreLogging]
        public virtual ICollection<StaffSemester> StaffSemesters { get; set; } = new List<StaffSemester>();
        [IgnoreLogging]
        public virtual ICollection<QualificationData> qualificationDatas { get; set; } = new List<QualificationData>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
