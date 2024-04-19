using GraduationProject.Data.Enum;
using GraduationProject.Service.DataTransferObject.PhoneDto;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.StudentDto
{
    public class UpdateStudentDto
    {
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string NameArabic { get; set; }
        [Required, MaxLength(500)]
        public string NameEnglish { get; set; }
        [Required, MaxLength(14)]
        [RegularExpression(@"^\d+$")]
        public string NationalID { get; set; }

        public string PlaceOfBirth { get; set; }

        public Gender Gender { get; set; }
        [Required, MaxLength(255)]
        public string StudentCode { get; set; }

        public Nationality Nationality { get; set; }

        public Religion Religion { get; set; }

        public string? ReleasePlace { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public int CountryId { get; set; }

        public int GovernorateId { get; set; }

        public int CityId { get; set; }

        public string? Street { get; set; }

        public string? PostalCode { get; set; }
        public string? PreQualification { get; set; }

        public int? SeatNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime? QualificationYear { get; set; }

        public decimal? Degree { get; set; }
        public string ParentName { get; set; }

        public string ParentJob { get; set; }

        public int ParentCountryId { get; set; }

        public int ParentGovernorateId { get; set; }


        public int ParentCityId { get; set; }


        public string? ParentStreet { get; set; }
        public List<PhoneNumberDto>? PhoneNumbers { get; set; }
    }
}
