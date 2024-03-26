using GraduationProject.Data.Enum;
using GraduationProject.Service.DataTransferObject.PhoneDto;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.StaffDto
{
    public class AddStaffDto
    {
        private int id;
        [Required, MaxLength(500)]
        public string NameArabic { get; set; }
        [Required, MaxLength(500)]
        public string NameEnglish { get; set; }
        [Required, MaxLength(14)]
        [RegularExpression(@"^\d+$")]
        public string NationalID { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword Not matched")]
        public string ConfirmPassword { get; set; }
        public string PlaceOfBirth { get; set; }

        public Gender Gender { get; set; }

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
        public List<PhoneNumberDto>? PhoneNumbers { get; set; }
    }
}
