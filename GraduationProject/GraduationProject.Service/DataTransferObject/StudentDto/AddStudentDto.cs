using GraduationProject.Data.Enum;
using GraduationProject.Service.DataTransferObject.PhoneDto;
using GraduationProject.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.StudentDto
{
    public class AddStudentDto
    {
        [IgnoreFromExcelFIle]
        public int? id { get; set; }

        [Required, MaxLength(500)]
        public string NameArabic { get; set; }

        [Required, MaxLength(500)]
        public string NameEnglish { get; set; }

        [Required, MaxLength(14), MinLength(14)]
        [RegularExpression(@"^\d+$", ErrorMessage = "NationalID must be number of 14 digit")]
        public string NationalID { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword not mated")]
        public string ConfirmPasswor { get; set; }

        public string PlaceOfBirth { get; set; }

        public Gender Gender { get; set; }

        public Nationality Nationality { get; set; }

        public Religion Religion { get; set; }
        public int FacultyId { get; set; }
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
        public string? PostalCodeOfParent { get; set; }

        [IgnoreFromExcelFIle]
        public List<PhoneNumberDto>? PhoneNumbers { get; set; }
    }

}
